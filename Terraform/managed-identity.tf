# Creation of an assigned identity to access the certificate for the application gateway
data "azurerm_user_assigned_identity" "uai" {
  resource_group_name = data.azurerm_resource_group.rg.name
  # location            = data.azurerm_resource_group.rg.location
  name = "s185-uai-cert-read"
}

# resource "azurerm_role_assignment" "uai-assignment" {
#   scope                = data.azurerm_key_vault.kv.id
#   role_definition_name = "Reader"
#   principal_id         = data.azurerm_user_assigned_identity.uai.principal_id
# }

# resource "azurerm_key_vault_access_policy" "uai-access-policy" {
#   key_vault_id = data.azurerm_key_vault.kv.id
#   tenant_id    = var.tenant_id
#   object_id    = data.azurerm_user_assigned_identity.uai.principal_id

#   key_permissions = [
#     "Get", "List",
#   ]

#   secret_permissions = [
#     "Get", "List",
#   ]

#   certificate_permissions = [
#     "Get", "List",
#   ]
# }

data "azurerm_user_assigned_identity" "appsauai" {
  resource_group_name = data.azurerm_resource_group.rg.name
  name                = var.azure_managed_identity_name[terraform.workspace]
}
