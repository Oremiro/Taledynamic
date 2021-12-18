import { UploadFileInfo } from "naive-ui";

interface OnBeforeUploadOptions {
  file: UploadFileInfo;
  fileList: UploadFileInfo[];
}

export type OnBeforeUpload = (options: OnBeforeUploadOptions) => Promise<boolean>;

interface OnFinishOptions {
  file: UploadFileInfo;
  event?: Event;
}

export type OnFinish = (options: OnFinishOptions) => UploadFileInfo | undefined;
