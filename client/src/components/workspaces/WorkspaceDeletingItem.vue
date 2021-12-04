<template>
  <n-popconfirm v-model:show="confirmShow">
    <template #icon>
      <n-icon :color="errorColor">
        <error-circle-icon />
      </n-icon>
    </template>
    <template #action>
      <n-button ghost type="error" size="small" @click.stop="deleteWorkspace"> Да </n-button>
      <n-button ghost size="small" @click.stop="confirmShow = false"> Нет </n-button>
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
</template>

<script setup lang="ts">
import { ref, computed } from "vue";
import { NPopconfirm, useMessage, useThemeVars } from "naive-ui";
import { useStore } from "@/store";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";
import { DeleteIcon, ErrorCircleIcon } from "@/components/icons";

const props = defineProps<{
  id: number;
}>();

const message = useMessage();

const store = useStore();
const confirmShow = ref<boolean>(false);

async function deleteWorkspace() {
  confirmShow.value = false;
  try {
    await store.dispatch("workspaces/delete", { id: props.id });
    message.success("Пространство удалено");
  } catch (error) {
    if (error instanceof Error) {
      message.error(error.message);
    }
  }
}

const errorColor = computed<string>(() => useThemeVars().value.errorColor);
</script>
