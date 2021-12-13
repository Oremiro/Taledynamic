<template>
  <n-page-header style="padding: 0 0.35rem" @back="backToMain">
    <template #title>
      <div style="display: flex; align-items: center">
        <n-button text @click="pushTable">
          <n-icon size="1.4rem">
            <save-icon />
          </n-icon>
        </n-button>
        <n-menu :value="currentTable?.id" mode="horizontal" :options="tablesListMenu" />
      </div>
    </template>
  </n-page-header>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, h } from "vue";
import { RouterLink, useRouter } from "vue-router";
import { NPageHeader, useMessage, MenuOption } from "naive-ui";
import { TableApi } from "@/helpers/api/table";
import { TableDto } from "@/models/api/responses";
import { useStore } from "@/store";
import { SaveIcon } from "@/components/icons";

const props = defineProps<{
  workspaceId: number;
  tableId: number;
}>();

const tablesList = ref<TableDto[]>([]);

const tablesListMenu = computed<MenuOption[]>(() =>
  tablesList.value.map((item) => ({
    label: () =>
      h(
        RouterLink,
        {
          to: {
            name: "Table",
            params: {
              workspaceId: props.workspaceId,
              tableId: item.id
            }
          }
        },
        { default: () => item.name }
      ),
    key: item.id
  }))
);

const currentTable = computed<TableDto | undefined>(() => tablesList.value.find((item) => item.id === props.tableId));

const store = useStore();
const message = useMessage();
async function setTableName(): Promise<void> {
  try {
    await store.dispatch("user/refreshExpired");
    const { data } = await TableApi.getList({ workspaceId: props.workspaceId }, store.getters["user/accessToken"]);
    tablesList.value = data.tables;
  } catch (error) {
    if (error instanceof Error) {
      message.error(error.message);
    }
  }
}

const router = useRouter();
function backToMain() {
  router.push({ name: "Main" });
}

async function pushTable(): Promise<void> {
  await store.dispatch("table/pushTable");
}

onMounted(async () => {
  await setTableName();
});
</script>
