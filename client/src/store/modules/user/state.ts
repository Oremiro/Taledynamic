import { UserState } from "@/models/store";

export const state: UserState = {
  user: {
    id: null,
    email: ""
  },
  accessTokenInMemory: {
    value: "",
    expiresAt: null
  }
};
