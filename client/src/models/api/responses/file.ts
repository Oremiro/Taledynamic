import BaseResponse from "@/models/api/responses/base";

export interface FileItem {
  id: string;
  base64String: string;
  type: string;
}

export interface FileCreateResponse extends BaseResponse {
  item: FileItem;
}

export interface FileGetFileResponse extends BaseResponse {}

export interface FileGetLinkResponse extends BaseResponse {
  item: FileItem;
}