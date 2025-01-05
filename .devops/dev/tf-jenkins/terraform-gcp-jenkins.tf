terraform {
  cloud {
    organization = "markma85-org"
    workspaces {
      name = "Innovate-Future-Foundation"
    }
  }
  required_providers {
    hcp = {
      source  = "hashicorp/hcp"
      version = "0.91.0"
    }
  }
}

variable "gcp_credentials" {
  type        = string
  sensitive   = true
  description = "GCP service account credentials"
}
variable "my_ssh_key" {
  type        = string
  sensitive   = true
  description = "My SSH public key"
}
variable "gcp_region" {
  type        = string
  description = "GCP region"
  default     = "us-central1"
}
variable "gcp_zone" {
  type        = string
  description = "GCP zone"
  default     = "us-central1-a"
}

# Providers
provider "google" {
  project     = "silver-charmer-444902-a9" # Replace with your GCP project ID
  credentials = var.gcp_credentials
  region      = var.gcp_region
  zone        = var.gcp_zone
}

# Resources
resource "google_compute_instance" "jenkins_vm" {
  name         = "jenkins-vm"
  machine_type = "e2-medium"
  zone         = "us-central1-a"

  tags = ["https-server", "ssh-server"]

  boot_disk {
    initialize_params {
      image = "ubuntu-os-cloud/ubuntu-2404-noble-amd64-v20241115"
    }
  }

  network_interface {
    network = "default"

    access_config {
      // Ephemeral public IP
    }
  }

  metadata = {
    ssh-keys = var.my_ssh_key
  }

  metadata_startup_script = file("startup.sh")

  scheduling {
    preemptible       = false
    automatic_restart = true
  }
}

resource "google_compute_firewall" "allow_https" {
  name    = "allow-https"
  network = "default"

  allow {
    protocol = "tcp"
    ports    = ["443"]
  }

  target_tags   = ["https-server"]
  source_ranges = ["0.0.0.0/0"]
}

resource "google_compute_firewall" "allow_ssh" {
  name    = "allow-ssh"
  network = "default"

  allow {
    protocol = "tcp"
    ports    = ["22"]
  }

  target_tags   = ["ssh-server"]
  source_ranges = ["0.0.0.0/0"]
}

output "public_ip" {
  description = "The public IP address of the Jenkins VM"
  value       = google_compute_instance.jenkins_vm.network_interface[0].access_config[0].nat_ip
}
