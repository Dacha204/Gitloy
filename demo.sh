#!/usr/bin/env bash

#!/usr/bin/env bash

########################################################################
### CONFIG
########################################################################

network="gitloy-demo.net"

demo_volume="gitloy-demo-volume"

web_i="httpd:2.4"
web_c="gitloy-demo.web"
web_port=8888

ftp_i="mauler/simple-ftp-server"
ftp_c="gitloy-demo.ftp"
ftp_port=21
ftp_user=gitloy
ftp_password=gitloy

remove_images=(
    # $web_i
    # $ftp_i
)

remove_containers=(
    $web_c
    $ftp_c
)

remove_networks=(
    $network
)

remove_volumes=(
    $demo_volume
)

########################################################################
########################################################################
########################################################################
### DOWN
########################################################################

function __remove_container {
    echo "GITLOY-DEMO: "
    echo "GITLOY-DEMO: Stopping $1 container"
    docker stop $1

    echo "GITLOY-DEMO: "
    echo "GITLOY-DEMO: Removing $1 container"
    docker rm $1
}

function __remove_image {
    echo "GITLOY-DEMO: "
    echo "GITLOY-DEMO: Removing $1 image"
    docker rmi $1
}

function __remove_network {
    echo "GITLOY-DEMO: "
    echo "GITLOY-DEMO: Remove $1 network"
    docker network remove $1
}

function __remove_volume {
    echo "GITLOY-DEMO: "
    echo "GITLOY-DEMO: Remove $1 volume"
    docker volume remove $1
}

function down {
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

########################################################################
### UP
########################################################################

function up {
    
    down

    echo "GITLOY-DEMO: Creating $demo_volume volume"
    docker volume create $demo_volume

    echo "GITLOY-DEMO: Creating $network network"
    docker network create $network

    #FTP SERVER
    echo "GITLOY-DEMO: Creating FTP Server"
    docker run -dit \
    --name $ftp_c \
    --network $network \
    -p $ftp_port:21 \
    -e FTP_USER=$ftp_user \
    -e FTP_PASS=$ftp_password \
    -v $demo_volume:/ftp-home/ \
    $ftp_i

    #WEB SERVER
    echo "GITLOY-DEMO: Creating Web Server"
    docker run -dit \
    --name $web_c \
    --network $network \
    -p $web_port:80 \
    -v $demo_volume:/usr/local/apache2/htdocs/ \
    $web_i

    echo ""
    echo ""
    echo "[FTP SERVER]"
    echo "Username = $ftp_user"
    echo "Password = $ftp_password"
    echo "Port = $ftp_port"
    echo ""
    echo "[WEB SERVER]"
    echo "Port = $web_port"
    echo ""
}

########################################################################
### COMMON
########################################################################

function print_usage {
    echo "Usage:  $0 up      =   Starts FTP/Web demo server"
    echo "        $0 down    =   Stops and removes demo servers"
}


function main {
    if [ $1 == "up" ]; then
        up
        exit 0
    fi

    if [ $1 = "down" ]; then
        down
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
