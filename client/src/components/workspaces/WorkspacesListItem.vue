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
            <checkmark-icon />
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
            <edit-icon />
          </n-icon>
        </n-button>
        <n-popconfirm v-model:show="confirmShow">
          <template #icon>
            <n-icon :color="errorColor">
              <error-circle-icon />
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
                <delete-icon />
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
import { ref, computed } from "vue";
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
import { Workspace } from "@/models/store";
import { stringValidator } from "@/helpers";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";
import { CheckmarkIcon, EditIcon, DeleteIcon, ErrorCircleIcon } from "@/components/icons";

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
      await stringValidator(workspaceName.value);
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

const errorColor = computed<string>(() => useThemeVars().value.errorColor);
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
