import { GlobalTheme } from "naive-ui";

export interface FormItemData<Type extends string | boolean> {
  value: Type;
  isValid?: boolean;
}

export interface SignUpFormData {
  email: FormItemData<string>;
  password: FormItemData<string>;
  confirmedPassword: FormItemData<string>;
}

export interface SignInFormData {
  email: FormItemData<string>;
  password: FormItemData<string>;
  remembered: FormItemData<boolean>;
}

export interface UserFieldEditFormData {
  currentPassword: FormItemData<string>;
}

export interface EmailEditFormData extends UserFieldEditFormData {
  email: FormItemData<string>;
}

export interface PasswordEditFormData extends UserFieldEditFormData {
  newPassword: FormItemData<string>;
  confirmedPassword: FormItemData<string>;
}

export type Theme = GlobalTheme | null;

export interface WorkspaceItem {
  id: number;
  isActive?: boolean;
  name: string;
  created?: Date;
  modified: Date;
}

export enum InitializationStatus {
  Pending,
  Success,
  Error
}