#!/bin/bash

# Create User
sudo adduser --disabled-password --gecos "" ${username}
sudo mkdir -p /home/${username}/.ssh
sudo touch /home/${username}/.ssh/authorized_keys
sudo cp /home/ubuntu/.ssh/authorized_keys /home/${username}/.ssh/
sudo chown -R ${username}:${username} /home/${username}/.ssh
sudo chmod 0600 /home/${username}/.ssh/authorized_keys
sudo usermod -aG sudo ${username}
sudo echo "${username} ALL=(ALL) NOPASSWD:ALL" | sudo tee /etc/sudoers.d/${username}

# Install Docker
echo "[INFO] Installing Docker client..."
sudo apt-get install ca-certificates curl -y
sudo install -m 0755 -d /etc/apt/keyrings
sudo curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
sudo chmod a+r /etc/apt/keyrings/docker.asc
echo \
    "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu \
    $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | \
    sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt-get update
sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin || exit_with_error "Docker client installation failed."
sudo apt clean
docker --version || exit_with_error "Docker client verification failed."

# Install Docker Compose
echo "[INFO] Installing Docker Compose..."
sudo curl -SL https://github.com/docker/compose/releases/download/v2.32.0/docker-compose-linux-x86_64 -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose
# Verify Docker Compose installation, if fails, create a symbolic link
docker-compose --version || sudo ln -s /usr/local/bin/docker-compose /usr/bin/docker-compose
# Verify Docker Compose installation
docker-compose --version || exit_with_error "Docker Compose installation failed."