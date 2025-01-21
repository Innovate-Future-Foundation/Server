variable "tags" {
    description = "The tags of the ECS cluster"
    type        = map(string)
    default     = {
        Environment = "dev"
        Project = "inff"
    }
}

################################################################################
# Cluster
################################################################################

variable "cluster_name" {
    description = "The name of the ECS cluster"
    type        = string
    default = "my-ecs-cluster"
}

variable "cluster_configuration" {
    description = "The configuration of the ECS cluster"
    type        = map
    default = {
        cluster_name = var.cluster_name
        capacity_providers = []
        default_capacity_provider_strategy = []
        settings = []
        tags = {}
    }
}

variable "cluster_settings" {
    description = "The settings of the ECS cluster"
    type        = list(map)
    default = [
        {
            name  = "containerInsights"
            value = "enabled"
        }
    ]
}

variable "cluster_service_connect_defaults" {
    description = "The service connect defaults of the ECS cluster"
    type        = list(map)
    default = [
        {
            container_name = "dotnet-app"
            container_port = 5091
            protocol       = "HTTP"
        },
        {
            container_name = "pgadmin"
            container_port = 5050
            protocol       = "HTTP"
        },
        {
            container_name = "postgre"
            container_port = 5432
            protocol       = "HTTP"
        }
    ]
}

################################################################################
# Capacity Providers
################################################################################

variable "default_capacity_provider_use_fargate" {
    description = "Whether to use Fargate as the default capacity provider"
    type        = bool
    default     = true
}

variable "fargate_capacity_providers" {
    description = "The Fargate capacity provider"
    type        = map
    default = {
        name = "FARGATE"
        platform_version = "1.4.0"
    }
}

variable "autoscaling_capacity_providers" {
    description = "The autoscaling capacity provider"
    type        = map
    default = {
        name = "EC2"
        managed_scaling = {
            status = "ENABLED"
            target_capacity = 70
        }
    }
}