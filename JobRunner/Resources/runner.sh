#!/bin/bash 
#Exit codes
# 1 = unknown
# 2 = failed to create clone dir
# 3 = failed to clone repo
# 4 = failed to upload files

function die() {
	echo "[`date "+%Y-%m-%d %H:%M:%S"`] ERROR: "$1 1>&2

	if [[ ! -z $2 ]]; then
		exit $2
	fi
	exit 1
}

function info() {
	echo "[`date "+%Y-%m-%d %H:%M:%S"`] INFO: "$1
}

function warrning() {
	echo "[`date "+%Y-%m-%d %H:%M:%S"`] WARRNING: "$1
}

info "Gitloy JobRunner started"

while getopts ":h:P:d:u:p:g:b:j:" option; do
case "${option}" in
h) FTP_HOST=$OPTARG;;
P) FTP_PORT=$OPTARG;;
d) FTP_DIR=$OPTARG;;
u) FTP_USER=$OPTARG;;
p) FTP_PASS=$OPTARG;;
g) GIT_URL=$OPTARG;;
b) GIT_BRANCH=$OPTARG;;
j) JOB_ID=$OPTARG;;
\?) die "Invalid option: -$OPTARG";;
\:)  die "Option "$OPTARG "require argument";;
esac
done

if [[ -z ${FTP_HOST} ]]; then
	die "[ftp] Host parameters is empty"
fi

if [[ -z ${FTP_PORT} ]]; then
	info "[ftp] Using default port 21"
	FTP_PORT=21
fi

if [[ -z ${FTP_DIR} ]]; then
	info "[ftp] Using ftp dir /"
	FTP_DIR="/"
fi

if [[ -z ${FTP_USER} ]]; then
	die "FTP Username not specified"
fi

if [[ -z ${FTP_PASS} ]]; then
	die "FTP Password not specified"
fi

if [[ -z ${GIT_URL} ]]; then
	die "GIT_URL not specified"
fi

if [[ -z ${GIT_BRANCH} ]]; then
	info "[git] Using master branch"
	GIT_BRANCH="master"
fi

if [[ -z ${JOB_ID} ]]; then
	die "JOB_ID not specified"
fi

CLONE_DIR=/tmp/$JOB_ID
SSH_PRIVATE=/root/.ssh/id_rsa
SSH_PUBLIC=/root/.ssh/id_rsa.pub

echo "==========================================="
info "[ftp] Hostname: "$FTP_HOST
info "[ftp] Port: "$FTP_PORT
info "[ftp] Base: "$FTP_DIR
info "[ftp] Username: "$FTP_USER
info "[ftp] Password: "$FTP_PASS
info "[git] GitURL: "$GIT_URL
info "[git] Branch: "$GIT_BRANCH
info "[job] JobId: "$JOB_ID
info "[job] CloneDir: "$CLONE_DIR
info "[job] PrivateKey: "$SSH_PRIVATE
info "[job] PublicKey: "$SSH_PUBLIC
echo "==========================================="

info "Cloning repo to "$CLONE_DIR

if [[ -d $CLONE_DIR ]]; then
	warrning $CLONE_DIR"already exists. Removing content.."
	rm -rf $CLONE_DIR
fi
mkdir -p $CLONE_DIR


if [[ $? -ne 0 ]]; then
	die "Failed to create clone dir" 2
fi

cd $CLONE_DIR

git clone $GIT_URL .
if [[ $? -ne 0 ]]; then
	die "Failed to clone repo" 3
fi

git-ftp push -u $FTP_USER -p $FTP_PASS -b $GIT_BRANCH --key $SSH_PRIVATE --pubkey $SSH_PUBLIC --insecure -f -v --auto-init ftp://$FTP_HOST:$FTP_PORT$FTP_DIR
if [[ $? -ne 0 ]]; then
	die "Failed to upload" 4
fi

info "Cleaning.."
rm -rf $CLONE_DIR
info "Job finished successfully"