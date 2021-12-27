import BaseResponse from "./base";

interface IntegrationTelegramItem {
  id: number;
  tgUserId: string;
  tgApiKey?: string;
}

export interface IntegrationTelegramAuthorizeResponse extends BaseResponse {
  item: IntegrationTelegramItem;
}

export interface IntegrationTelegramGetResponse extends BaseResponse {
  item: IntegrationTelegramItem;
}

export interface IntegrationTelegramRevokeResponse extends BaseResponse {}
