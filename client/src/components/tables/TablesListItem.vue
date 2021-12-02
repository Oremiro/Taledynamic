<template>
  <transition name="fade" mode="out-in">
    <n-button-group v-if="!isInputShown">
      <n-button @click="navigateToTable">
        {{ name }}
      </n-button>
      <n-button
        v-if="editable"
        style="padding: 0.5rem"
        @click="isInputShown = true"
      >
        <n-icon size="1.2rem">
          <edit-icon />
        </n-icon>
      </n-button>
      <n-popconfirm v-if="editable" v-model:show="isDeleteConfirmationShown">
        <template #icon>
          <n-icon :color="errorColor">
            <error-circle-icon />
          </n-icon>
        </template>
        <template #action>
          <n-button ghost type="error" size="small" @click="deleteTable">
            Да
          </n-button>
          <n-button
            ghost
            size="small"
            @click="isDeleteConfirmationShown = false"
          >
            Нет
          </n-button>
        </template>
        <template #trigger>
          <dynamically-typed-button style="padding: 0.5rem" type="error" ghost>
            <n-icon size="1.2rem">
              <delete-icon />
            </n-icon>
          </dynamically-typed-button>
        </template>
        <div>Удалить таблицу?</div>
      </n-popconfirm>
    </n-button-group>
    <table-name-item
      v-else
      :default-name="props.name"
      :loading="isInputLoading"
      @blur="isInputShown = false"
      @valid="editTableName"
    />
  </transition>
</template>

<script setup lang="ts">
import axios from "axios";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { useMessage, useThemeVars } from "naive-ui";
import { TableApi } from "@/helpers/api/table";
import { useStore } from "@/store";
import { TableDto } from "@/models/api/responses";
import { DeleteIcon, EditIcon, ErrorCircleIcon } from "@/components/icons";
import TableNameItem from "@/components/tables/TableNameItem.vue";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";

const props = defineProps<{
  id: number;
  workspaceId: number;
  name: string;
  editable?: boolean;
}>();

const emit = defineEmits<{
  (e: "update", oldId: number, table: TableDto): void;
  (e: "delete", id: number): void;
}>();

const isInputShown = ref<boolean>(false);
const isInputLoading = ref<boolean>(false);
const store = useStore();

async function editTableName(name: string): Promise<void> {
  if (name === props.name) {
    isInputShown.value = false;
    return;
  }
  try {
    isInputLoading.value = true;
    await store.dispatch("user/refreshExpired")
    const { data } = await TableApi.update(
      {
        name: name,
        id: props.id
      },
      store.getters["user/accessToken"]
    );
    emit("update", props.id, data.table);
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.log(error.message);
    }
  } finally {
    isInputLoading.value = false;
    isInputShown.value = false;
  }
}

const message = useMessage();
const errorColor = computed<string>(() => useThemeVars().value.errorColor);
const isDeleteConfirmationShown = ref<boolean>(false);

async function deleteTable(): Promise<void> {
  try {
    await store.dispatch("user/refreshExpired")
    await TableApi.delete({ id: props.id }, store.getters["user/accessToken"]);
    emit("delete", props.id);
    message.success("Таблица удалена");
  } catch (error) {
    if (axios.isAxiosError(error)) {
      console.log(error.message);
    }
  }
}

const router = useRouter();

function navigateToTable() {
  router.push({
    name: "Table",
    params: { workspaceId: props.workspaceId, tableId: props.id }
  });
}
</script>
