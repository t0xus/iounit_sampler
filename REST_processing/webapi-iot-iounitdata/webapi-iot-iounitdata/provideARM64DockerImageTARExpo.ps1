#######################################################################################
#ATTENTION: You need a QEMU ARM64 Builder. Before execute on a x64 System for example, 
#download the right Builder image from official Docker repository.
#######################################################################################

#Delete all previously created build files (including temporary ones).
Remove-Item -Path "bin\*" -Recurse -Force
Remove-Item -Path "obj\*" -Recurse -Force

#Create the Build with the Image and use dockerfile in current folder.
docker buildx build --platform linux/arm64 -t webapi-iot-iounitdata --load .

#Save the Image as Export TAR File for Linux Systems (for example ARM64 Raspberry PI OS)
docker save -o webapi-iot-iounitdata.tar webapi-iot-iounitdata:latest

Write-Host "Press any key to continue..."