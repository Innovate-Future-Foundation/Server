provider "aws" {
  region = local.region
}

data "aws_availability_zones" "available" {
  state = "available"
}

locals {
  region       = "ap-southeast-2"
  cluster_name = "inff-ecs-cluster"

  vpc_cidr_block     = "10.0.0.0/16"
  availability_zones = slice(data.aws_availability_zones.available.names, 0, 2)

  containers = {
    container_migration : {

    },
    container_postgres : {
      ports : [
        5432, 5050
      ]
    },
    container_inff-server : {
      ports : [
        5091
      ]
    }
  }
  tags = {
    Project     = "inff"
    Name        = "Backend Web Server"
    Environment = "dev"
  }
}

module "ecs_cluster" {
  source = "modules/ecs-cluster"

  cluster_name = local.cluster_name
  fargate_capacity_providers = {
    FARGATE = {
      default_capacity_provider_strategy = {
        capacity_provider = "FARGATE"
        weight            = 50
        base              = 20
      }
    }
    FARGATE_SPOT = {
      default_capacity_provider_strategy = {
        capacity_provider = "FARGATE_SPOT"
        weight            = 50
      }
    }
  }
}
