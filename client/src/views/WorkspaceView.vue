<template>
  <div class="container" style="padding: 4rem 0">
    <n-card style="max-width: 40rem">
      <template #header>
        <n-page-header v-if="isInitializationSuccess">
          <template #title>
            <n-text type="success">{{ currentWorkspace?.name }}</n-text>
          </template>
          <template #subtitle>
            <div>
              Дата изменения: {{ currentWorkspace?.modified.toLocaleString() }}
            </div>
          </template>
          <template #extra>
            <div style="display: flex; gap: .5rem">
              <n-button text>
                <n-icon size="1.2rem">
                  <edit-icon />
                </n-icon>
              </n-button>
              <n-button text>
                <n-icon size="1.2rem">
                  <arrow-sort-icon />
                </n-icon>
              </n-button>
            </div>
          </template>
        </n-page-header>
        <n-skeleton v-else text round />
        <n-divider style="margin-bottom: 0" />
      </template>
      <template #default>
        <tables-list :workspace-id="workspaceId" />
      </template>
    </n-card>
  </div>
</template>

<script setup lang="ts">
import { computed, watch, onMounted } from "vue";
import { NPageHeader, NDivider, NSkeleton } from "naive-ui";
import { useRouter } from "vue-router";
import { useStore } from "@/store";
import { Workspace } from "@/interfaces/store";
import { InitializationStatus } from "@/interfaces";
import TablesList from "@/components/tables/TablesList.vue";
import { ArrowSortIcon, EditIcon } from "@/components/icons";

const props = defineProps({
  id: {
    type: String,
    required: true
  }
});

const workspaceId = computed<number>(() => parseInt(props.id));

const store = useStore();
const workspaces = computed<Workspace[]>(
  () => store.getters["workspaces/workspaces"]
);
const currentWorkspace = computed<Workspace | null>(
  () => store.getters["workspaces/currentWorkspace"]
);

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

const isInitializationSuccess = computed<boolean>(() => workspacesInitStatus.value === InitializationStatus.Success)

/* Setting current workspace */
const workspacesInitStatus = computed<InitializationStatus>(
  () => store.getters["workspaces/initStatus"]
);
onMounted(async () => {
  if (isInitializationSuccess.value) {
    await setCurrentWorkspace(workspaceId.value);
  }
});
watch(workspacesInitStatus, async (value) => {
  if (value === InitializationStatus.Success) {
    await setCurrentWorkspace(workspaceId.value);
  }
});
watch(workspaceId, async (value) => {
  await setCurrentWorkspace(value);
});
</script>
