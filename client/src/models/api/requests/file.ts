export interface FileCreateRequest {
  fileBase64: string;
  type: string;
}

export interface FileGetFileRequest {
  uId: string;
}

export interface FileGetLinkRequest {
  uId: string;
}