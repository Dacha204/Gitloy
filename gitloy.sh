#!/usr/bin/env bash

########################################################################
### CONFIG
########################################################################

domain="localhost"
network="gitloy.net"
wait_for_broker=5s
wait_for_db=5s

broker_i="rabbitmq:3.7.8-management-alpine"
broker_c="gitloy.rabbitmq"

db_i="microsoft/mssql-server-linux:2017-latest"
db_c="gitloy.db"

worker_i="gitloy/worker"
worker_c="gitloy.worker"

portal_i="gitloy/portal"
portal_c="gitloy.portal"

webhook_i="gitloy/webhook"
webhook_c="gitloy.webhook"

loadbalancer_i="abiosoft/caddy:0.11.0"
loadbalancer_c="gitloy.lb"
loadbalancer_v="v.gitloy.loadbalancer"

remove_images=(
    $worker_i
    $webhook_i
    $portal_i
    # $db_i
    # $broker_i
    # $loadbalancer_i
)

remove_containers=(
    $worker_c
    $webhook_c
    $portal_c
    $db_c
    $broker_c
    $loadbalancer_c
)

remove_networks=(
    $network
)

remove_volumes=(
    # $loadbalancer_v
)

########################################################################
########################################################################
########################################################################
### CLEAN
########################################################################

function __remove_container {
    echo "GITLOY: "
    echo "GITLOY: Stopping $1 container"
    docker stop $1

    echo "GITLOY: "
    echo "GITLOY: Removing $1 container"
    docker rm $1
}

function __remove_image {
    echo "GITLOY: "
    echo "GITLOY: Removing $1 image"
    docker rmi $1
}

function __remove_network {
    echo "GITLOY: "
    echo "GITLOY: Remove $1 network"
    docker network remove $1
}

function __remove_volume {
    echo "GITLOY: "
    echo "GITLOY: Remove $1 volume"
    docker volume remove $1
}

function uninstall {
    for i in ${remove_containers[@]}; do
        __remove_container $i   
    done

    for i in ${remove_images[@]}; do
        __remove_image $i   
    done

    for i in ${remove_networks[@]}; do
        __remove_network $i   
    done
    
    for i in ${remove_volumes[@]}; do
        __remove_volume $i   
    done
}

function stop {
    for i in ${remove_containers[@]}; do
        __remove_container $i   
    done
}

########################################################################
### INSTALL
########################################################################
function set_domain {
    sed -i "1s/.*/$1/" Caddyfile
    domain=$(head -n 1 Caddyfile)
    echo $"GITLOY: Domain set to: $domain"
}

function install {
    uninstall
    build
    set_domain $1
    run
}

########################################################################
### RUN
########################################################################

function run {

    #RABBITMQ
    echo "GITLOY: Running $broker_c"
    docker run -dit \
    --name $broker_c \
    --hostname $broker_c \
    --network $network \
    -e RABBITMQ_DEFAULT_USER="gitloy" \
    -e RABBITMQ_DEFAULT_PASS="P@ssword123!" \
    -p 15672:15672 \
    $broker_i

    echo "GITLOY: Waiting $wait_for_broker to broker initialize"
    sleep $wait_for_broker

    #SQL DATABASE
    echo "GITLOY: Running $db_c"
    docker run -dit \
    --name $db_c \
    --network $network \
    -e 'ACCEPT_EULA=Y' \
    -e 'SA_PASSWORD=P@ssword123!' \
    -p 14330:1433 \
    $db_i

    echo "GITLOY: Waiting $wait_for_db to database initialize"
    sleep $wait_for_db

    #WORKER
    echo "GITLOY: Running $worker_c"
    docker run -dit \
    --name $worker_c \
    --network $network \
    -e "ASPNETCORE_ENVIRONMENT=Docker" \
    $worker_i

    #PORTAL
    echo "GITLOY: Running $portal_c"
    docker run -dit \
    --name $portal_c \
    --network $network \
    -e "ASPNETCORE_ENVIRONMENT=Docker" \
    -p 20480:80 \
    $portal_i

    #WEBHOOK API
    echo "GITLOY: Running $webhook_c"
    docker run -dit \
    --name $webhook_c \
    --network $network \
    -e "ASPNETCORE_ENVIRONMENT=Docker" \
    -p 20481:80 \
    $webhook_i

    #LOADBALANCER/REVERSEPROXY
    echo "GITLOY: Running $loadbalancer_c"
    mkdir -p ~/.caddy
    docker run -dit \
    --name $loadbalancer_c \
    --network $network \
    -p 80:80 \
    -p 443:443 \
    -p 2015:2015 \
    -v $(pwd)/Caddyfile:/etc/Caddyfile \
    -v $HOME/.caddy:/root/.caddy \
    $loadbalancer_i
}

########################################################################
### BUILD
########################################################################

function build {
    
    #NETWORK
    echo "GITLOY: Creating network $network"
    docker network create \
    --subnet 10.24.0.0/24 \
    --gateway 10.24.0.1 \
    $network

    #VOLUME
    echo "GITLOY: Create volume $loadbalancer_v"
    docker volume create $loadbalancer_v

    #WORKER
    echo "GITLOY: Build $worker_i"
    docker build -t $worker_i -f JobRunner/Dockerfile .
    
    #PORTAL
    echo "GITLOY: Build $portal_i"
    docker build -t $portal_i -f FrontPortal/Dockerfile .
    
    #WEBHOOK API
    echo "GITLOY: Build $webhook_i"
    docker build -t $webhook_i -f WebhookAPI/Dockerfile .
}

########################################################################
### COMMON
########################################################################

function print_usage {
    echo "GITLOY: Usage: $0 build                   =   builds required images"
    echo "GITLOY:        $0 install <domain-name>   =   build and run gitloy environment"
    echo "GITLOY:        $0 uninstall               =   stops & removes all resource that gitloy creates"
    echo "GITLOY:        $0 run                     =   runs containers"
    echo "GITLOY:        $0 stop                    =   stops and removes containers"
    echo "GITLOY:        $0 domain <domain-name>    =   update domain after install"
}


function main {
    if [ $1 == "build" ]; then
        build
        exit 0
    fi

    if [ $1 = "install" ]; then
        if [ -z $2 ]; then
            echo "GITLOY: Missing domain parameter"
            exit 1
        fi        
        install $2
        exit 0
    fi

    if [ $1 = "uninstall" ]; then
        uninstall
        exit 0
    fi

    if [ $1 = "run" ]; then
        run
        exit 0
    fi

    if [ $1 = "stop" ]; then
        stop
        exit 0
    fi
    
    if [ $1 = "domain" ]; then
        if [ -z $2 ]; then
            echo "GITLOY: missing domain parameter"
            exit 1
        fi
        set_domain $2
        exit 0
    fi

    print_usage
    exit 1
}

if [ $# -eq 0 ]; then
    print_usage
    exit 1
else 
    main $1 $2 $3
fi