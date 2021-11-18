<template>
  <transition name="fade" mode="out-in" @enter="doAfterTransition">
    <div
      v-if="isNameInputShown"
      style="display: flex; align-items: center; justify-content: space-between"
    >
      <n-form-item
        :show-label="false"
        :show-feedback="false"
        :rule="workspaceNameRule"
      >
        <n-input
          ref="nameInput"
          v-model:value="workspaceName"
          :loading="isNameInputLoading"
          :disabled="isNameInputLoading"
          size="small"
          placeholder="Название"
          @keyup.enter="editWorkspaceName"
          @focusout.stop="isNameInputShown = false"
        />
      </n-form-item>
      <div style="display: flex; align-items: center; margin-left: 1rem">
        <n-button
          v-if="isWorkspaceNameValid && workspaceName !== name"
          text
          style="margin-right: 0.3rem"
          @click="editWorkspaceName"
        >
          <n-icon size="1.2rem">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              xmlns:xlink="http://www.w3.org/1999/xlink"
              viewBox="0 0 24 24"
            >
              <g fill="none">
                <path
                  d="M4.53 12.97a.75.75 0 0 0-1.06 1.06l4.5 4.5a.75.75 0 0 0 1.06 0l11-11a.75.75 0 0 0-1.06-1.06L8.5 16.94l-3.97-3.97z"
                  fill="currentColor"
                />
              </g>
            </svg>
          </n-icon>
        </n-button>
        <div v-else style="width: 1.5rem" />
        <dynamically-typed-button type="error" text>
          <n-icon size="1.2rem">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              xmlns:xlink="http://www.w3.org/1999/xlink"
              viewBox="0 0 24 24"
            >
              <g fill="none">
                <path
                  d="M4.397 4.554l.073-.084a.75.75 0 0 1 .976-.073l.084.073L12 10.939l6.47-6.47a.75.75 0 1 1 1.06 1.061L13.061 12l6.47 6.47a.75.75 0 0 1 .072.976l-.073.084a.75.75 0 0 1-.976.073l-.084-.073L12 13.061l-6.47 6.47a.75.75 0 0 1-1.06-1.061L10.939 12l-6.47-6.47a.75.75 0 0 1-.072-.976l.073-.084l-.073.084z"
                  fill="currentColor"
                />
              </g>
            </svg>
          </n-icon>
        </dynamically-typed-button>
      </div>
    </div>
    <div
      v-else
      style="display: flex; justify-content: space-between; align-items: center"
    >
      <n-ellipsis :tooltip="{ delay: 500, placement: 'top-end' }">
        <router-link :to="toLink">
          {{ name }}
        </router-link>
        <template #tooltip>
          {{ name }}
        </template>
      </n-ellipsis>
      <div style="display: flex; align-items: center; margin-left: 1rem">
        <n-button
          text
          style="margin-right: 0.3rem"
          @click="isNameInputShown = true"
        >
          <n-icon size="1.2rem">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              xmlns:xlink="http://www.w3.org/1999/xlink"
              viewBox="0 0 24 24"
            >
              <g fill="none">
                <path
                  d="M21.03 2.97a3.578 3.578 0 0 1 0 5.06L9.062 20a2.25 2.25 0 0 1-.999.58l-5.116 1.395a.75.75 0 0 1-.92-.921l1.395-5.116a2.25 2.25 0 0 1 .58-.999L15.97 2.97a3.578 3.578 0 0 1 5.06 0zM15 6.06L5.062 16a.75.75 0 0 0-.193.333l-1.05 3.85l3.85-1.05A.75.75 0 0 0 8 18.938L17.94 9L15 6.06zm2.03-2.03l-.97.97L19 7.94l.97-.97a2.079 2.079 0 0 0-2.94-2.94z"
                  fill="currentColor"
                />
              </g>
            </svg>
          </n-icon>
        </n-button>
        <n-popconfirm v-model:show="confirmShow">
          <template #icon>
            <n-icon size="1.5rem" :color="errorColor">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                xmlns:xlink="http://www.w3.org/1999/xlink"
                viewBox="0 0 24 24"
              >
                <g fill="none">
                  <path
                    d="M12 2c5.523 0 10 4.478 10 10s-4.477 10-10 10S2 17.522 2 12S6.477 2 12 2zm0 1.667c-4.595 0-8.333 3.738-8.333 8.333c0 4.595 3.738 8.333 8.333 8.333c4.595 0 8.333-3.738 8.333-8.333c0-4.595-3.738-8.333-8.333-8.333zm-.001 10.835a.999.999 0 1 1 0 1.998a.999.999 0 0 1 0-1.998zM11.994 7a.75.75 0 0 1 .744.648l.007.101l.004 4.502a.75.75 0 0 1-1.493.103l-.007-.102l-.004-4.501a.75.75 0 0 1 .75-.751z"
                    fill="currentColor"
                  />
                </g>
              </svg>
              <!-- <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 20 20"><g fill="none"><path d="M10 2a8 8 0 1 1 0 16a8 8 0 0 1 0-16zm0 1a7 7 0 1 0 0 14a7 7 0 0 0 0-14zm0 9.5a.75.75 0 1 1 0 1.5a.75.75 0 0 1 0-1.5zM10 6a.5.5 0 0 1 .492.41l.008.09V11a.5.5 0 0 1-.992.09L9.5 11V6.5A.5.5 0 0 1 10 6z" fill="currentColor"></path></g></svg> -->
            </n-icon>
          </template>
          <template #action>
            <n-button ghost type="error" size="small" @click="deleteWorkspace">
              Да
            </n-button>
            <n-button ghost size="small" @click="confirmShow = false">
              Нет
            </n-button>
          </template>
          <template #trigger>
            <dynamically-typed-button type="error" text @click.stop>
              <n-icon size="1.2rem">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  xmlns:xlink="http://www.w3.org/1999/xlink"
                  viewBox="0 0 28 28"
                >
                  <g fill="none">
                    <path
                      d="M14 2.25a3.75 3.75 0 0 1 3.745 3.55l.005.2h5.5a.75.75 0 0 1 .102 1.493l-.102.007h-1.059l-1.22 15.053A3.75 3.75 0 0 1 17.233 26h-6.466a3.75 3.75 0 0 1-3.738-3.447L5.808 7.5H4.75a.75.75 0 0 1-.743-.648L4 6.75a.75.75 0 0 1 .648-.743L4.75 6h5.5A3.75 3.75 0 0 1 14 2.25zm6.687 5.25H7.313l1.211 14.932a2.25 2.25 0 0 0 2.243 2.068h6.466a2.25 2.25 0 0 0 2.243-2.068L20.686 7.5zm-8.937 3.75a.75.75 0 0 1 .743.648L12.5 12v8a.75.75 0 0 1-1.493.102L11 20v-8a.75.75 0 0 1 .75-.75zm4.5 0a.75.75 0 0 1 .743.648L17 12v8a.75.75 0 0 1-1.493.102L15.5 20v-8a.75.75 0 0 1 .75-.75zM14 3.75a2.25 2.25 0 0 0-2.245 2.096L11.75 6h4.5l-.005-.154A2.25 2.25 0 0 0 14 3.75z"
                      fill="currentColor"
                    />
                  </g>
                </svg>
              </n-icon>
            </dynamically-typed-button>
          </template>
          <div>Удалить пространство?</div>
        </n-popconfirm>
      </div>
    </div>
  </transition>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { RouterLink, useRouter } from "vue-router";
import {
  FormItemRule,
  NEllipsis,
  NInput,
  NPopconfirm,
  useMessage,
  useThemeVars
} from "naive-ui";
import { useStore } from "@/store";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";
import { Workspace } from "@/interfaces/store";
import { workspaceNameValidator } from "@/helpers";

const props = defineProps({
  id: {
    type: Number,
    required: true
  },
  name: {
    type: String,
    required: true
  }
});

const workspaceName = ref<string>(props.name);
const nameInput = ref<InstanceType<typeof NInput>>();
const isNameInputShown = ref<boolean>(false);

function doAfterTransition(): void {
  if (isNameInputShown.value) {
    nameInput.value?.focus();
  }
}

const isWorkspaceNameValid = ref<boolean>(false);

const workspaceNameRule: FormItemRule = {
  trigger: "input",
  asyncValidator: async (): Promise<void> => {
    isWorkspaceNameValid.value = false;
    try {
      await workspaceNameValidator(workspaceName.value);
      isWorkspaceNameValid.value = true;
    } catch (error) {
      if (error instanceof Error) {
        throw error;
      }
    }
  }
};

const isNameInputLoading = ref<boolean>(false);

async function editWorkspaceName(): Promise<void> {
  if (workspaceName.value === props.name || !isWorkspaceNameValid.value) {
    isNameInputShown.value = false;
    workspaceName.value = props.name;
    return;
  }
  isNameInputLoading.value = true;
  try {
    const currentWorkspaceId: number =
      store.getters["workspaces/currentWorkspace"]?.id;
    await store.dispatch("workspaces/update", {
      id: props.id,
      name: workspaceName.value
    });
    if (props.id === currentWorkspaceId) {
      router.push({
        name: "Workspace",
        params: { id: store.getters["workspaces/currentWorkspace"]?.id }
      });
    }
  } catch (error) {
    if (error instanceof Error) {
      message.error(error.message);
    }
  } finally {
    isNameInputShown.value = false;
    isNameInputLoading.value = false;
  }
}

const { errorColor } = useThemeVars().value;
const confirmShow = ref<boolean>(false);

const toLink = `/workspace/${props.id}`;

const store = useStore();
const message = useMessage();
const router = useRouter();

async function deleteWorkspace() {
  confirmShow.value = false;
  try {
    await store.dispatch("workspaces/delete", { id: props.id });
    message.success("Пространство удалено");
    const currentWorkspace: Workspace | null =
      store.getters["workspaces/currentWorkspace"];
    if (currentWorkspace?.id === props.id) {
      const workspaces: Workspace[] = store.getters["workspaces/workspaces"];
      if (workspaces.length) {
        router.push({ name: "Workspace", params: { id: workspaces[0].id } });
      } else {
        router.push({ name: "Home" });
      }
    }
  } catch (error) {
    if (error instanceof Error) {
      message.error(error.message);
    }
  }
}
</script>
