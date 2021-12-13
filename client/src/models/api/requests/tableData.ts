export interface TableDataGetRequest {
  id: number;
}

export interface TableDataCreateRequest {
  id: number;
  jsonContent: string;
}

export interface TableDataUpdateRequest {
  uId: string;
  jsonContent: string;
}

export interface TableDataDeleteRequest {
  uId: string;
}
