resource "azurerm_user_assigned_identity" "uai" {
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  name                = "s185-uai-cert-read"
}

resource "azurerm_role_assignment" "uai-assignment" {
  scope                = data.azurerm_key_vault.kv.id
  role_definition_name = "Reader"
  principal_id         = azurerm_user_assigned_identity.uai.principal_id
}

resource "azurerm_key_vault_access_policy" "uai-access-policy" {
  key_vault_id = data.azurerm_key_vault.kv.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_user_assigned_identity.uai.principal_id

  key_permissions = [
    "Get", "List",
  ]

  secret_permissions = [
    "Get", "List",
  ]

  certificate_permissions = [
    "Get", "List",
  ]
}
