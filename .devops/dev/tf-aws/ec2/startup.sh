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

# Enable Docker Remote Access on Port 2375
echo "[INFO] Configuring Docker to enable remote access on port 2375..."
sudo mkdir -p /etc/docker
sudo bash -c 'cat << EOF > /etc/docker/daemon.json
{
  "hosts": ["unix:///var/run/docker.sock", "tcp://0.0.0.0:2375"]
}
EOF'

# Update docker.service to remove the '-H fd://' conflict
echo "[INFO] Updating Docker service configuration..."
sudo sed -i "s|-H fd://||g" /usr/lib/systemd/system/docker.service

# Reload systemd and restart Docker
sudo systemctl daemon-reload
sudo systemctl restart docker

# Install net-tools if 'netstat' not found
echo "[INFO] Checking for 'netstat' command..."
if ! command -v netstat &> /dev/null; then
    echo "[INFO] 'netstat' not found. Installing net-tools..."
    sudo apt-get install -y net-tools || exit_with_error "Failed to install net-tools."
else
    echo "[INFO] 'netstat' command is already available."
fi

# Verify if Docker is listening on port 2375
if sudo netstat -tuln | grep 2375; then
    echo "[INFO] Docker is now accessible on port 2375."
else
    echo "[ERROR] Failed to enable Docker remote access on port 2375."
    exit 1
fi

# Add user to the docker group
echo "[INFO] Adding user ${username} to the docker group..."
sudo usermod -aG docker ${username}

# Install Docker Compose
echo "[INFO] Installing Docker Compose..."
sudo curl -SL https://github.com/docker/compose/releases/download/v2.32.0/docker-compose-linux-x86_64 -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose
# Verify Docker Compose installation, if fails, create a symbolic link
docker-compose --version || sudo ln -s /usr/local/bin/docker-compose /usr/bin/docker-compose
# Verify Docker Compose installation
docker-compose --version || exit_with_error "Docker Compose installation failed."

# Run Portainer
echo "[INFO] Running Portainer..."
sudo docker volume create portainer_data
sudo docker run -d -p 8000:8000 -p 9443:9443 --name portainer --restart=always -v /var/run/docker.sock:/var/run/docker.sock portainer/portainer-ce:latest