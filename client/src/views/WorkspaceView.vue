<template>
  <div class="container" style="padding: 4rem 0">
    <n-card style="max-width: 40rem;">
      <template #header>
        <n-page-header>
          <template #title>
            <div>{{ currentWorkspace?.name }}</div>
          </template>
          <template #subtitle>
            <div>Дата изменения: {{ currentWorkspace?.modified.toLocaleString() }}</div>
          </template>
          <template #extra>
            <n-button text>
              <n-icon size="1.2rem">
                <arrow-sort-icon />
              </n-icon>
            </n-button>
          </template>
        </n-page-header>
      </template>
      <template #default>
        <tables-list :workspace-id="workspaceId" /> 
      </template>
    </n-card>
  </div>
</template>

<script setup lang="ts">
import { computed, watch, onMounted } from "vue";
import { NPageHeader } from "naive-ui";
import { useRouter } from "vue-router";
import { useStore } from "@/store";
import { Workspace } from "@/interfaces/store";
import { InitializationStatus } from "@/interfaces";
import ArrowSortIcon from "@/components/icons/ArrowSortIcon.vue";
import TablesList from "@/components/tables/TablesList.vue";

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
const workspacesInitStatus = computed<InitializationStatus>(() => store.getters["workspaces/initStatus"]);
onMounted(async () => {
  if (workspacesInitStatus.value === InitializationStatus.Success) {
    await setCurrentWorkspace(workspaceId.value)
  }
})
watch(workspacesInitStatus, async (value) => {
  if (value === InitializationStatus.Success) {
    await setCurrentWorkspace(workspaceId.value);
  }
})
watch(workspaceId, async (value) => {
  await setCurrentWorkspace(value);
});
</script>
