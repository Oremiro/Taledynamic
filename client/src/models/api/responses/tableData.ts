import BaseResponse from "@/models/api/responses/base";

export interface TableDataItem {
  uId: string;
  tableData: string;
}

export interface TableDataGetResponse extends BaseResponse {
  item: TableDataItem;
}

export interface TableDataCreateResponse extends BaseResponse {
  item: TableDataItem;
}

export interface TableDataUpdateResponse extends BaseResponse {}

export interface TableDataDeleteResponse extends BaseResponse {}
