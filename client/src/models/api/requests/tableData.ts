import { TableJson } from "@/models/table";

export interface TableDataGetRequest {
  id: number;
}

export interface TableDataCreateRequest {
  id: number;
  jsonContent: TableJson;
}

export interface TableDataUpdateRequest {
  uId: string;
  jsonContent: TableJson;
}

export interface TableDataDeleteRequest {
  uId: string;
}
