<template>
  <transition name="fade" mode="out-in" @enter="doAfterTransition">
    <delayed-button
      v-if="!isWorkspaceInputShown"
      ref="workspaceCreatingButton"
      :disabled="disabled || isWorkspaceCreatingPending"
      :loading="isWorkspaceCreatingPending"
      @click="isWorkspaceInputShown = true"
    >
      {{
        isWorkspaceCreatingPending
          ? "Создание пространства"
          : "Создать пространство"
      }}
    </delayed-button>
    <n-form-item
      v-else
      :show-label="false"
      :show-feedback="false"
      :rule="workspaceCreatingRule"
    >
      <n-input-group>
        <n-input
          ref="nameInput"
          v-model:value="newWorkspaceName"
          placeholder="Название пространства"
          @keyup.enter="createWorkspace"
        />
        <n-button
          v-if="isWorkspaceInputValid"
          attr-type="submit"
          style="padding: 0.6rem"
          @click="createWorkspace"
        >
          <n-icon size="1.2rem">
            <checkmark-icon />
          </n-icon>
        </n-button>
        <dynamically-typed-button
          ghost
          style="padding: 0.6rem"
          type="error"
          @click="clearNameInput"
        >
          <n-icon size="1.2rem">
            <dismiss-icon />
          </n-icon>
        </dynamically-typed-button>
      </n-input-group>
    </n-form-item>
  </transition>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import { FormItemRule, NInput, NInputGroup, useMessage } from "naive-ui";
import { useStore } from "@/store";
import { workspaceNameValidator } from "@/helpers";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";
import DelayedButton from "@/components/DelayedButton.vue";
import CheckmarkIcon from "@/components/icons/CheckmarkIcon.vue";
import DismissIcon from "@/components/icons/DismissIcon.vue";

defineProps({
  disabled: {
    type: Boolean,
    default: false
  }
});

const newWorkspaceName = ref<string>("");
const isWorkspaceInputValid = ref<boolean>(false);
const workspaceCreatingRule: FormItemRule = {
  trigger: "input",
  asyncValidator: async (): Promise<void> => {
    isWorkspaceInputValid.value = false;
    try {
      await workspaceNameValidator(newWorkspaceName.value);
      isWorkspaceInputValid.value = true;
    } catch (error) {
      if (error instanceof Error) {
        throw error;
      }
    }
  }
};

const isWorkspaceInputShown = ref<boolean>(false);

const nameInput = ref<InstanceType<typeof NInput>>();

function doAfterTransition(): void {
  if (isWorkspaceInputShown.value) {
    nameInput.value?.focus();
  }
}

function clearNameInput(): void {
  newWorkspaceName.value = "";
  isWorkspaceInputShown.value = false;
}

const store = useStore();
const message = useMessage();
const creatingButtonHolding = ref<boolean>(false);
const isWorkspaceCreatingPending = ref<boolean>(false);

async function createWorkspace(): Promise<void> {
  isWorkspaceInputShown.value = false;
  if (!isWorkspaceInputValid.value) {
    return;
  }
  isWorkspaceCreatingPending.value = true;
  try {
    await store.dispatch("workspaces/create", { name: newWorkspaceName.value });
    message.success(`Рабочее пространство ${newWorkspaceName.value} создано`);
  } catch (error) {
    if (error instanceof Error) {
      creatingButtonHolding.value = true;
      message.error(error.message);
    }
  } finally {
    isWorkspaceCreatingPending.value = false;
    isWorkspaceInputValid.value = false;
    newWorkspaceName.value = "";
  }
}

const workspaceCreatingButton = ref<InstanceType<typeof DelayedButton>>();

watch(workspaceCreatingButton, (value): void => {
  if (creatingButtonHolding.value) {
    // @ts-expect-error: vue-next #4397
    value?.holdDisabled();
    creatingButtonHolding.value = false;
  }
});
</script>
