param location string = resourceGroup().location
param storageAccountName string
param videoIndexerAccountName string

var storageBlobDataContributorRoleId = 'ba92f5b4-2d11-453d-a403-e96b0029c9fe' 
var roleAssignmentGuid = guid('FairPlayCombined','VideoIndexer','Storage Blob Data Contributor')

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-06-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
    minimumTlsVersion: 'TLS1_2'
  }
}

resource videoIndexer 'Microsoft.VideoIndexer/accounts@2024-01-01' = {
    name: videoIndexerAccountName
    location: location
    identity: {
        type: 'SystemAssigned'
    }

    properties: {
        storageServices : {
            resourceId : storageAccount.id
        }
    }
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
    scope: storageAccount
	name: roleAssignmentGuid
	
	properties: {
		roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', storageBlobDataContributorRoleId)
		principalId: videoIndexer.identity.principalId
		principalType: 'ServicePrincipal'
	}
}