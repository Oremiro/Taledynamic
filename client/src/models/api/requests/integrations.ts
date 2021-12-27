export interface IntegrationTelegramAuthorizeRequest {
  telegramUserId: string;
  telegramApiKey?: string;
}
export interface IntegrationTelegramRevokeRequest {
  id: number;
}