################################################################################
# Cluster
################################################################################

locals {

}

resource "aws_ecs_cluster" "this" {
  name = var.cluster_name

  dynamic "service_connect_defaults" {
    for_each = length(var.cluster_service_connect_defaults) > 0 ? [var.cluster_service_connect_defaults] : []
    
    content {
        namespace = service_connect_defaults.value.namespace
    }
  }

  dynamic "setting" {
    for_each = flattern([var.cluster_settings])
    
    content {
        name  = setting.value.name
        value = setting.value.value
    }
  }

  tags = var.tags
}

################################################################################
# Cluster Capacity Providers
################################################################################

locals {
    default_capacity_providers = merge(
        {}
    )
}