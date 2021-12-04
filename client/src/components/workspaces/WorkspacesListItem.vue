<template>
  <transition name="fade" mode="out-in" @enter="doAfterTransition">
    <div v-if="isNameInputShown" style="display: flex; align-items: center; justify-content: space-between">
      <n-form-item :show-label="false" :show-feedback="false" :rule="workspaceNameRule">
        <n-input
          ref="nameInput"
          v-model:value="workspaceName"
          :maxlength="100"
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
        <dynamically-typed-button type="error" text @click.stop>
          <n-icon size="1.2rem">
            <dismiss-icon />
          </n-icon>
        </dynamically-typed-button>
      </div>
    </div>
    <div v-else style="display: flex; justify-content: space-between; align-items: center">
      <n-ellipsis :tooltip="{ delay: 500, placement: 'top-end' }">
        {{ name }}
        <template #tooltip>
          {{ name }}
        </template>
      </n-ellipsis>
      <div style="display: flex; align-items: center; margin-left: 1rem">
        <n-button text style="margin-right: 0.3rem" @click.stop="isNameInputShown = true">
          <n-icon size="1.2rem">
            <edit-icon />
          </n-icon>
        </n-button>
        <workspace-deleting-item :id="id" />
      </div>
    </div>
  </transition>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { FormItemRule, NEllipsis, NInput, useMessage } from "naive-ui";
import { useStore } from "@/store";
import { stringValidator } from "@/helpers";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";
import { CheckmarkIcon, EditIcon, DismissIcon } from "@/components/icons";
import { useRouter } from "vue-router";
import WorkspaceDeletingItem from "@/components/workspaces/WorkspaceDeletingItem.vue";

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

const router = useRouter();

async function editWorkspaceName(): Promise<void> {
  if (workspaceName.value === props.name || !isWorkspaceNameValid.value) {
    isNameInputShown.value = false;
    workspaceName.value = props.name;
    return;
  }
  isNameInputLoading.value = true;
  try {
    const currentWorkspaceId: number | null = store.getters["workspaces/currentWorkspaceId"];
    await store.dispatch("workspaces/update", {
      id: props.id,
      name: workspaceName.value
    });
    if (props.id === currentWorkspaceId) {
      router.push({
        name: "Main",
        params: { id: store.getters["workspaces/currentWorkspaceId"] }
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

const store = useStore();
const message = useMessage();
</script>
