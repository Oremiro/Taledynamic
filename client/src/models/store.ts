import { InitializationStatus } from "@/models";
import { TableHeader, TableRow } from "@/models/table";

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
  currentWorkspace: Workspace | null;
  sortType: WorkspacesSortType;
  initStatus: InitializationStatus
}

export enum WorkspacesSortType {
  NameAscending,
  NameDescending,
  DateAscending,
  DateDescending
}

export interface TableState {
  headers: TableHeader[]
  rows: TableRow[]
}