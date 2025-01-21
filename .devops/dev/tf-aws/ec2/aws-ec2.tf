variable "username" {
  type        = string
  description = "The login username for the EC2 instance"
  default     = "hyperion"
}

provider "aws" {
  region = "ap-southeast-2" # Change this to your desired AWS region
}

variable "subnet_id" {
  type        = string
  sensitive   = false
  default     = "subnet-047f11180977a1dc2"
  description = "The ID of the subnet where the EC2 instance will be deployed."
}

resource "aws_key_pair" "key_pair" {
  key_name   = "hyperion_pub_key"
  public_key = file("~/.ssh/id_rsa.pub")
}

resource "aws_security_group" "web_sg" {
  name_prefix = "web-sg-"
  description = "Security group for backend web server"

  ingress {
    description = "Allow SSH access"
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    description = "Allow HTTP access"
    from_port   = 5091
    to_port     = 5091
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    description = "Allow HTTPS access"
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    description = "Allow Docker access"
    from_port   = 2375
    to_port     = 2375
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  ingress {
    description = "Allow Postgres access"
    from_port   = 5432
    to_port     = 5432
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  ingress {
    description = "Allow PgAdmin access"
    from_port   = 5050
    to_port     = 5050
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    description = "Allow Portainer access"
    from_port   = 9443
    to_port     = 9443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_instance" "backend_web_server" {
  ami                    = data.aws_ami.ubuntu.id
  instance_type          = "t2.micro"
  key_name               = aws_key_pair.key_pair.key_name
  subnet_id              = var.subnet_id
  vpc_security_group_ids = [aws_security_group.web_sg.id]

  root_block_device {
    volume_size = 8
    volume_type = "gp2"
  }
  tags = {
    Project     = "inff"
    Name        = "Backend Web Server"
    Environment = "dev"
  }

  user_data = templatefile("startup.sh", {
    username = var.username
  })

  provisioner "file" {
    source      = "../../pgadmin/servers.json"
    destination = "/home/${var.username}/servers.json"
    connection {
      type        = "ssh"
      host        = self.public_ip
      user        = var.username
      private_key = file("~/.ssh/id_rsa")
    }
  }
  provisioner "remote-exec" {
    inline = [
      "sudo mkdir -p /opt/pgadmin",
      "sudo mv /home/${var.username}/servers.json /opt/pgadmin/servers.json",
      "sudo chown -R ${var.username}:${var.username} /opt/pgadmin",
    ]
    connection {
      type        = "ssh"
      host        = self.public_ip
      user        = var.username
      private_key = file("~/.ssh/id_rsa")
    }
  }
}

data "aws_ami" "ubuntu" {
  most_recent = true
  owners      = ["099720109477"] # Canonical's AWS Account ID
  filter {
    name   = "name"
    values = ["ubuntu/images/hvm-ssd*/ubuntu-noble-24.04-amd64-server-*"]
  }
}

data "aws_route53_zone" "mark_it_zone" {
  name         = "mark-it-zone.com"
  private_zone = false
}

resource "aws_route53_record" "backend_a_record" {
  zone_id = data.aws_route53_zone.mark_it_zone.id
  name    = "inff-backend.mark-it-zone.com"
  type    = "A"
  ttl     = 300
  records = [aws_instance.backend_web_server.public_ip]
}

# output "ubuntu_ami_id" {
#   description = "The public IP address of the Jenkins VM"
#   value       = data.aws_ami.ubuntu
# }

output "ec2_public_ip" {
  value       = aws_instance.backend_web_server.public_ip
  description = "The public IP address of the EC2 instance"
}
