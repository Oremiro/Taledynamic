import { InitializationStatus } from "@/models";
import { TableHeader, TableRow, TableRowsSortType } from "@/models/table";

export interface User {
  id: number | null;
  email: string;
}

interface UpdatedBaseData {
  currentPassword: string;
}

export interface UpdatedEmailData extends UpdatedBaseData {
  newEmail: string;
}

export interface UpdatedPasswordData extends UpdatedBaseData {
  newPassword: string;
  confirmedNewPassword: string;
}

export interface State {
  pageStatus: "loading" | "ready" | "error";
}

export interface UserState {
  user: User;
  accessTokenInMemory: {
    value: string;
    expiresAt: Date | null;
  };
}

export interface LoginState {
  user: User;
  accessTokenInMemory: string;
}

export interface Workspace {
  id: number;
  name: string;
  created: Date;
  modified: Date;
  userId: number;
}

export interface WorkspacesState {
  workspaces: Workspace[];
  currentWorkspaceId: number | null;
  sortType: WorkspacesSortType;
  initStatus: InitializationStatus;
}

export enum WorkspacesSortType {
  NameAscending,
  NameDescending,
  DateAscending,
  DateDescending
}

export interface TableSortStatus {
  index: number;
  type: TableRowsSortType;
}

export interface TableState {
  dataId?: string;
  headers: TableHeader[];
  rows: TableRow[];
  editableRowIndex?: number;
  editableHeaderIndex?: number;
  sortStatus?: TableSortStatus;
  isUpdated: boolean;
  immutable: boolean;
}
