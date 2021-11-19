<template>
  <div class="container" style="padding: 4rem 0">
    <n-grid :cols="4" style="max-width: 50rem; column-gap: 2rem;">
      <n-grid-item :span="3">
        <n-card>
          <template #header>
            <n-page-header>
              <template #title>
                <div>{{ currentWorkspace?.name }}</div>
              </template>
              <template #subtitle>
                <div>Дата изменения: {{ currentWorkspace?.modified.toLocaleString() }}</div>
              </template>
            </n-page-header>
          </template>
          <template #default>
            <n-scrollbar>
              <n-button v-for="item in 100" :key="item" style="margin: .5rem 1rem">Таблица #{{ item }}</n-button>
            </n-scrollbar>
            <table-creating-item />
          </template>
        </n-card>
      </n-grid-item>
      <n-grid-item :span="1">
        <n-card>
          Сортировка
        </n-card>
      </n-grid-item>
    </n-grid>
  </div>
</template>

<script setup lang="ts">
import { computed, watch, onMounted } from "vue";
import { NPageHeader, NScrollbar } from "naive-ui";
import { useRouter } from "vue-router";
import { useStore } from "@/store";
import { Workspace, WorkspacesInitStatus } from "@/interfaces/store";
import TableCreatingItem from "@/components/tables/TableCreatingItem.vue";

const props = defineProps({
  id: {
    type: String,
    required: true
  }
});

const workspaceId = computed<number>(() => parseInt(props.id));

const store = useStore();
const workspaces = computed<Workspace[]>(() => store.getters["workspaces/workspaces"]);
const currentWorkspace = computed<Workspace | null>(() => store.getters["workspaces/currentWorkspace"]);

const router = useRouter();

async function setCurrentWorkspace(id: number): Promise<void> {
  if (isNaN(id)) {
    router.push({ name: "NotFound" });
    return;
  }
  const currentWorkspace: Workspace | undefined = workspaces.value.find(
    (item) => item.id === id
  );
  if (currentWorkspace) {
    store.commit("workspaces/setCurrent", { workspace: currentWorkspace });
  } else {
    router.push({ name: "NotFound" });
  }
}

/* Setting current workspace */
const workspacesInitStatus = computed<WorkspacesInitStatus>(() => store.getters["workspaces/initStatus"]);
onMounted(async () => {
  if (workspacesInitStatus.value === WorkspacesInitStatus.Success) {
    await setCurrentWorkspace(workspaceId.value)
  }
})
watch(workspacesInitStatus, async (value) => {
  if (value === WorkspacesInitStatus.Success) {
    await setCurrentWorkspace(workspaceId.value);
  }
})
watch(workspaceId, async (value) => {
  await setCurrentWorkspace(value);
});
</script>
