#!/bin/bash
echo "test wifi"
# bcast=$(ifconfig | grep 10.5.5 | tr -s ' '| cut -d ' ' -f4 | cut -d ':' -f2)
# if [ "$bcast" != "10.5.5.255" ]
# then
# 	echo "not connected to camera wifi"
# 	exit 4
# fi
nc -z 10.5.5.9 8080
if [ $? == 1 ]
then
	echo "camera off, turning on"
	curl "http://10.5.5.9/bacpac/PW?t=ludoques&p=%01"
fi
until nc -z 10.5.5.9 8080 >> /dev/null 2>&1; do echo "waiting for camera ready"; sleep 1; done
# until nc -z 10.5.5.9 8080; do echo "waiting for camera ready"; sleep 5; done

echo "taking picture!"
curl "http://10.5.5.9/bacpac/SH?t=ludoques&p=%01" >> /dev/null 2>&1
if [ $? != 0 ]
then
	echo "take picture failed"
fi
sleep 3
wget -r -A .JPG -Nc -p fotos--cut-dirs=3 http://10.5.5.9:8080/DCIM/101GOPRO/ >> /dev/null 2>&1
echo "wget returned $?"
