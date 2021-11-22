import BaseResponse from "@/interfaces/api/responses/base";

export interface TableDto {
  id: number;
  name: string;
}

export interface TableCreateResponse extends BaseResponse {
  table: TableDto;
}

export interface TableDeleteResponse extends BaseResponse {}

export interface TableUpdateResponse extends BaseResponse {
  table: TableDto;
}

export interface TableGetResponse extends BaseResponse {
  table: TableDto;
}

export interface TableGetListResponse extends BaseResponse {
  tables: TableDto[];
}
