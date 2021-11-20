<template>
  <transition name="fade" mode="out-in">
    <n-button-group v-if="!isInputShown">
      <n-button> {{ name }} </n-button>
      <n-button
        v-if="editable"
        style="padding: 0.5rem"
        @click="isInputShown = true;"
      >
        <n-icon size="1.2rem">
          <edit-icon />
        </n-icon>
      </n-button>
      <dynamically-typed-button
        v-if="editable"
        style="padding: 0.5rem"
        type="error"
        ghost
      >
        <n-icon size="1.2rem">
          <delete-icon />
        </n-icon>
      </dynamically-typed-button>
    </n-button-group>
    <table-name-item
      v-else
      :default-name="props.name"
      :loading="isInputLoading"
      compare
      @blur="isInputShown = false;"
      @valid="editTableName"
    />
  </transition>
</template>

<script setup lang="ts">
import axios from "axios";
import { ref } from "vue";
import { TableApi } from "@/helpers/api/table";
import { useStore } from "@/store";
import { TableDto } from "@/interfaces/api/responses";
import { DeleteIcon, EditIcon } from "@/components/icons";
import TableNameItem from "@/components/tables/TableNameItem.vue";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";

const props = defineProps<{
  id: number;
  name: string;
  editable?: boolean;
}>();

const emit = defineEmits<{
  (e: "update", oldId: number, table: TableDto): void;
}>();

const isInputShown = ref<boolean>(false);
const isInputLoading = ref<boolean>(false);
const store = useStore();

async function editTableName(name: string): Promise<void> {
  try {
    isInputLoading.value = true;
    const { data } = await TableApi.update(
      {
        name: name,
        id: props.id
      },
      store.getters["user/accessToken"]
    );
    emit("update", props.id, data.table);
  } catch (error) {
    // TODO: 401 Handler
    if (axios.isAxiosError(error)) {
      console.log(error.message);
    }
  } finally {
    isInputLoading.value = false;
    isInputShown.value = false
  }
}
</script>
