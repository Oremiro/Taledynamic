<template>
  <transition
    name="fade"
    mode="out-in"
  >
    <workspace-loading
      v-if="isListLoading"
      :error="isListLoadingError"
      @repeat-loading="getWorkspaces"
    />
    <div
      v-else
      class="workspaces-section-content"
    >
      <div class="workspaces-section-content-header">
        <n-text depth="3">
          Ваши рабочие пространства
        </n-text>
        <n-popselect
          v-model:value="popSortValue"
          trigger="click"
          :options="popOptions"
          @update:value="updateHandler"
        >
          <n-button text>
            <n-icon size="1.2rem">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                xmlns:xlink="http://www.w3.org/1999/xlink"
                viewBox="0 0 24 24"
              ><g fill="none"><path
                d="M17.25 4l-.1.007a.75.75 0 0 0-.65.743v12.692l-3.22-3.218l-.084-.072a.75.75 0 0 0-.976 1.134l4.504 4.5l.084.072a.75.75 0 0 0 .976-.073l4.497-4.5l.072-.084a.75.75 0 0 0-.073-.977l-.084-.072a.75.75 0 0 0-.977.073L18 17.446V4.75l-.006-.102A.75.75 0 0 0 17.251 4zm-11.036.22L1.72 8.715l-.073.084a.75.75 0 0 0 .073.976l.084.073a.75.75 0 0 0 .976-.073l3.217-3.218v12.698l.008.102a.75.75 0 0 0 .743.648l.101-.007a.75.75 0 0 0 .649-.743L7.497 6.559l3.223 3.217l.084.072a.75.75 0 0 0 .975-1.134L7.275 4.22l-.085-.072a.75.75 0 0 0-.976.073z"
                fill="currentColor"
              /></g></svg>
            </n-icon>
          </n-button>
        </n-popselect>
      </div>
      <n-scrollbar style="height: 100%;">
        <workspaces-list :workspaces="workspaces" />
      </n-scrollbar>
      <div class="workspaces-section-content-footer">
        <workspace-creating-button />
      </div>
    </div>
  </transition>
</template>


<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { SelectGroupOption, SelectOption, NPopselect, NScrollbar } from 'naive-ui';
import { useStore } from '@/store';
import { Workspace, WorkspacesSortType } from '@/interfaces/store';
import WorkspaceCreatingButton from '@/components/workspaces/WorkspaceCreating.vue';
import WorkspacesList from '@/components/workspaces/WorkspacesList.vue'
import WorkspaceLoading from '@/components/workspaces/WorkspacesLoading.vue'


const store = useStore();

const workspaces = computed<Workspace[]>(() => store.getters['workspaces/workspaces']);
const popOptions: Array<SelectOption | SelectGroupOption> = [
	{
		type: 'group',
		label: 'По дате',
		key: 'sortByDate',
		children: [
			{
				label: 'Сначала новые',
				value: WorkspacesSortType.DateDescending
			},
			{
				label: 'Сначала старые',
				value: WorkspacesSortType.DateAscending
			},
		]
	},
	{
		type: 'group',
		label: 'По названию',
		key: 'sortByName',
		children: [
			{
				label: 'От A до Z',
				value: WorkspacesSortType.NameAscending
			},
			{
				label: 'От Z до A',
				value: WorkspacesSortType.NameDescending
			},
		]
	}
]

const popSortValueStored: string | null = localStorage.getItem('workspacesSort');
const popSortValueParsed: number = popSortValueStored ? parseInt(popSortValueStored) : WorkspacesSortType.DateDescending;
const popSortValue = ref<WorkspacesSortType>(!isNaN(popSortValueParsed) ? popSortValueParsed : WorkspacesSortType.DateDescending);

const isListLoading = ref<boolean>(workspaces.value.length ? false : true);
const isListLoadingError = ref<boolean>(false);


async function updateHandler(value: number): Promise<void> {
	localStorage.setItem('workspacesSort', value.toString());
	await store.dispatch('workspaces/sort', { sortType: value });
}

async function getWorkspaces(): Promise<void> {
	isListLoadingError.value = false;
	try {
		await store.dispatch('workspaces/init');
		isListLoading.value = false;
	} catch (error) {
		if (error instanceof Error) {
			isListLoadingError.value = true;
		}
	}
}

onMounted(async (): Promise<void> => {
	if(!workspaces.value.length) {
		setTimeout(async (): Promise<void> => {
			await getWorkspaces();
			await store.dispatch('workspaces/sort', { sortType: popSortValue.value });
		}, 500);
	} else {
		await store.dispatch('workspaces/sort', { sortType: popSortValue.value });
	}
})
</script>

<style lang="scss" scoped>
.workspaces-section-content {
	height: 100%; 
	display: flex; 
	flex-direction: column;
}

.workspaces-section-content-header {
	padding: 1rem; 
	display: flex; 
	align-items: center; 
	justify-content: space-between;
}
.workspaces-section-content-footer {
	padding: 1rem; 
	display: flex; 
	align-items: center; 
	justify-content: center;
}
</style>