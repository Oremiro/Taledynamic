<template>
  <transition name="fade" mode="out-in">
    <n-button
      v-if="!isInputShown"
      ghost
      :type="type"
      @click="isInputShown = true;"
    >
      <slot />
    </n-button>
    <table-name-item
      v-else
      :default-name="defaultTableName"
      :loading="isInputLoading"
      @blur="isInputShown = false"
      @valid="createTable"
    />
  </transition>
</template>

<script setup lang="ts">
import axios from "axios";
import { ref, computed } from "vue";
import { useMessage } from "naive-ui";
import { TableApi } from "@/helpers/api/table";
import { useStore } from "@/store";
import { TableDto } from "@/interfaces/api/responses";
import TableNameItem from "@/components/tables/TableNameItem.vue";

interface Props {
  workspaceId: number;
  tablesCount: number;
  type?: "default" | "primary" | "success" | "info" | "warning" | "error";
}

const emit = defineEmits<{
  (e: "create", table: TableDto): void;
}>();

const props = withDefaults(defineProps<Props>(), {
  type: "default"
});

const isInputShown = ref<boolean>(false);
const defaultTableName = computed<string>(
  () => `Table #${props.tablesCount + 1}`
);
const isInputLoading = ref<boolean>(false);

const store = useStore();
const message = useMessage();

async function createTable(name: string): Promise<void> {
  if (props.tablesCount >= 100) {
    message.error(
      "В одном рабочем пространстве не может быть более 100 таблиц"
    );
    return;
  }
  try {
    isInputLoading.value = true;
    const { data } = await TableApi.create(
      {
        name: name,
        workspaceId: props.workspaceId
      },
      store.getters["user/accessToken"]
    );
    emit("create", data.table);
    message.success("Таблица успешно создана");
  } catch (error) {
    // TODO: 401 Handler
    if (axios.isAxiosError(error)) {
      console.log(error.message);
    }
  } finally {
    isInputLoading.value = false;
    isInputShown.value = false;
  }
}
</script>
