# Definition of the dns zone for the applications
resource "azurerm_private_dns_zone" "dnsprivatezone" {
  name                = "privatelink.azurewebsites.net"
  resource_group_name = data.azurerm_resource_group.rg.name

  tags = {
    Environment = lookup(data.azurerm_resource_group.rg.tags, "Environment", "")
    Portfolio   = lookup(data.azurerm_resource_group.rg.tags, "Portfolio", "")
    Product     = lookup(data.azurerm_resource_group.rg.tags, "Product", "")
    Service     = lookup(data.azurerm_resource_group.rg.tags, "Service", "")
  }
}

# Definition of the dns zone for the storage account
resource "azurerm_private_dns_zone" "dnsprivatezone-sa" {
  name                = "privatelink.blob.core.windows.net"
  resource_group_name = data.azurerm_resource_group.rg.name

  tags = {
    Environment = lookup(data.azurerm_resource_group.rg.tags, "Environment", "")
    Portfolio   = lookup(data.azurerm_resource_group.rg.tags, "Portfolio", "")
    Product     = lookup(data.azurerm_resource_group.rg.tags, "Product", "")
    Service     = lookup(data.azurerm_resource_group.rg.tags, "Service", "")
  }
}

# Definition of the dns zone link for the applications
resource "azurerm_private_dns_zone_virtual_network_link" "dnszonelink" {
  name                  = var.private_dns_zone_vn_link_name[terraform.workspace]
  resource_group_name   = data.azurerm_resource_group.rg.name
  private_dns_zone_name = azurerm_private_dns_zone.dnsprivatezone.name
  virtual_network_id    = data.azurerm_virtual_network.vnet1.id

  tags = {
    Environment = lookup(data.azurerm_resource_group.rg.tags, "Environment", "")
    Portfolio   = lookup(data.azurerm_resource_group.rg.tags, "Portfolio", "")
    Product     = lookup(data.azurerm_resource_group.rg.tags, "Product", "")
    Service     = lookup(data.azurerm_resource_group.rg.tags, "Service", "")
  }
}

# Definition of the dns zone link for the storage account
resource "azurerm_private_dns_zone_virtual_network_link" "dnszonelink-sa" {
  name                  = "${var.private_dns_zone_vn_link_name[terraform.workspace]}-sa"
  resource_group_name   = data.azurerm_resource_group.rg.name
  private_dns_zone_name = azurerm_private_dns_zone.dnsprivatezone-sa.name
  virtual_network_id    = data.azurerm_virtual_network.vnet1.id

  tags = {
    Environment = lookup(data.azurerm_resource_group.rg.tags, "Environment", "")
    Portfolio   = lookup(data.azurerm_resource_group.rg.tags, "Portfolio", "")
    Product     = lookup(data.azurerm_resource_group.rg.tags, "Product", "")
    Service     = lookup(data.azurerm_resource_group.rg.tags, "Service", "")
  }
}

# Definition of the private end point for the web app
resource "azurerm_private_endpoint" "privateendpoint" {
  name                = var.private_endpoint_name[terraform.workspace]
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  subnet_id           = azurerm_subnet.backend.id

  private_dns_zone_group {
    name                 = var.private_dns_zone_group_name[terraform.workspace]
    private_dns_zone_ids = [azurerm_private_dns_zone.dnsprivatezone.id]
  }

  private_service_connection {
    name                           = var.private_endpoint_conn_name[terraform.workspace]
    private_connection_resource_id = azurerm_linux_web_app.linux-web-app.id
    subresource_names              = ["sites"]
    is_manual_connection           = false
  }

  tags = data.azurerm_resource_group.rg.tags
}

# Definition of the private end point for the AI Search
resource "azurerm_private_endpoint" "searchprivateendpoint" {
  name                = var.search_private_endpoint_name[terraform.workspace]
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  subnet_id           = azurerm_subnet.backend.id

  # Only create when we are not on a free tier (Prod/Load-Test)
  count = local.with_basic_aisearch_count

  private_service_connection {
    name                           = var.search_private_endpoint_conn_name[terraform.workspace]
    private_connection_resource_id = azurerm_search_service.ai-search[0].id
    subresource_names              = ["searchService"]
    is_manual_connection           = false
  }

  tags = data.azurerm_resource_group.rg.tags
}

# Definition of the private end point for the grafana web app
resource "azurerm_private_endpoint" "privateendpoint-gf" {
  name                = "${var.private_endpoint_name[terraform.workspace]}-gf"
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  subnet_id           = azurerm_subnet.backend.id

  private_dns_zone_group {
    name                 = var.private_dns_zone_group_name[terraform.workspace]
    private_dns_zone_ids = [azurerm_private_dns_zone.dnsprivatezone.id]
  }

  private_service_connection {
    name                           = "${var.private_endpoint_conn_name[terraform.workspace]}-gf"
    private_connection_resource_id = azurerm_linux_web_app.grafana-web-app.id
    subresource_names              = ["sites"]
    is_manual_connection           = false
  }

  tags = data.azurerm_resource_group.rg.tags
}

data "azurerm_storage_account" "webappsa" {
  name                = var.cpd_azure_storage_account[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
}

# Definition of the private end point for the application storage
resource "azurerm_private_endpoint" "privateendpoint-sa" {
  name                = "${var.private_endpoint_name[terraform.workspace]}-sa"
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  subnet_id           = azurerm_subnet.backend.id

  private_dns_zone_group {
    name                 = var.private_dns_zone_group_name[terraform.workspace]
    private_dns_zone_ids = [azurerm_private_dns_zone.dnsprivatezone-sa.id]
  }

  private_service_connection {
    name                           = "${var.private_endpoint_conn_name[terraform.workspace]}-sa"
    private_connection_resource_id = data.azurerm_storage_account.webappsa.id
    subresource_names              = ["blob"]
    is_manual_connection           = false
  }

  tags = data.azurerm_resource_group.rg.tags
}
