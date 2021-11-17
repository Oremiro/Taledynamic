<template>
  <n-form ref="formRef" :rules="rules" :model="formData">
    <n-form-item ref="emailInputRef" first label="Email" path="email.value">
      <n-auto-complete
        v-slot="{ handleInput, handleBlur, handleFocus, value }"
        v-model:value="formData.email.value"
      >
        <n-input
          placeholder=""
          :value="value"
          :loading="isEmailValidationPending"
          @input="handleInput"
          @focus="handleFocus"
          @blur="handleBlur"
        >
          <template v-if="!formData.email.isValid" #prefix>
            <question-tooltip>
              Email может содержать только буквы латинского алфавита, цифры,
              точку, подчеркивание и минус. Почтовый домен должен быть
              корректным.
            </question-tooltip>
          </template>
        </n-input>
      </n-auto-complete>
    </n-form-item>
    <n-collapse-transition :show="isSubmitCollapseShown">
      <n-form-item label="Текущий пароль" path="currentPassword.value">
        <n-input
          v-model:value="formData.currentPassword.value"
          type="password"
          show-password-on="click"
          placeholder=""
        />
      </n-form-item>
      <delayed-button
        ref="submitButtonRef"
        attr-type="submit"
        type="success"
        ghost
        style="margin-right: 1rem"
        :disabled="
          isSubmitButtonLoading ||
          isEmailValidationPending ||
          !formData.email.isValid ||
          !formData.currentPassword.value
        "
        :loading="isSubmitButtonLoading"
        @click="submitForm"
      >
        Сохранить
      </delayed-button>
      <n-button ghost type="error" @click="undoChanges"> Отменить </n-button>
    </n-collapse-transition>
  </n-form>
</template>

<script setup lang="ts">
import { debounce, emailRegex } from "@/helpers";
import { FormRules, NForm, NFormItem, useMessage } from "naive-ui";
import { reactive, ref } from "vue";
import { useStore } from "@/store";
import { EmailEditFormData } from "@/interfaces";
import QuestionTooltip from "@/components/QuestionTooltip.vue";
import DelayedButton from "@/components/DelayedButton.vue";
import { UserApi } from "@/helpers/api/user";
import axios from "axios";

const store = useStore();
const formRef = ref<InstanceType<typeof NForm>>();
const defaultEmailValue = ref<string>(store.getters["user/email"]);
const formData = reactive<EmailEditFormData>({
  email: {
    value: defaultEmailValue.value,
    isValid: true
  },
  currentPassword: {
    value: "",
    isValid: false
  }
});

const isEmailValidationPending = ref<boolean>(false);
const rules: FormRules = {
  email: {
    value: [
      {
        required: true,
        asyncValidator: debounce(
          async (rule, value) => {
            if (value === defaultEmailValue.value) {
              formData.email.isValid = true;
              isSubmitCollapseShown.value = false;
              isEmailValidationPending.value = false;
              return;
            } else if (!emailRegex.test(value)) {
              formData.email.isValid = false;
              isSubmitCollapseShown.value = false;
              isEmailValidationPending.value = false;
              throw new Error("Введите корректный email");
            }
            try {
              const { data } = await UserApi.isEmailUsed({ email: value });
              isEmailValidationPending.value = false;
              if (data.isEmailUsed) {
                formData.email.isValid = false;
                isSubmitCollapseShown.value = false;
                throw new Error("Данный email занят другим пользователем");
              } else {
                formData.email.isValid = true;
                isSubmitCollapseShown.value = true;
              }
            } catch (error) {
              isEmailValidationPending.value = false;
              if (axios.isAxiosError(error)) {
                throw new Error(error.response?.statusText);
              } else {
                throw error;
              }
            }
          },
          1000,
          {
            isAwaited: true,
            immediateFunc: () => {
              isEmailValidationPending.value = true;
            }
          }
        ),
        trigger: "input"
      }
    ]
  },
  currentPassword: {
    value: [
      {
        required: true,
        message: "Введите текущий пароль",
        trigger: "input"
      }
    ]
  }
};
const emailInputRef = ref<InstanceType<typeof NFormItem>>();
const submitButtonRef = ref<InstanceType<typeof DelayedButton>>();
const message = useMessage();
const isSubmitCollapseShown = ref<boolean>(false);
const isSubmitButtonLoading = ref<boolean>(false);

function undoChanges(): void {
  formData.email.value = defaultEmailValue.value;
  formData.currentPassword.value = "";
  isSubmitCollapseShown.value = false;
  emailInputRef.value?.restoreValidation();
}
function saveChanges(): void {
  formData.currentPassword.value = "";
  defaultEmailValue.value = store.getters["user/email"];
  isSubmitCollapseShown.value = false;
}
function submitForm(): void {
  isSubmitButtonLoading.value = true;
  formRef.value?.validate(async (errors) => {
    if (!errors) {
      try {
        await store.dispatch("user/updateEmail", {
          currentPassword: formData.currentPassword.value,
          newEmail: formData.email.value
        });
        saveChanges();
        message.success("Вы успешно изменили email");
      } catch (error) {
        if (error instanceof Error) {
          message.error(error.message);
        }
      }
    } else {
      message.error("Данные не являются корректными");
    }
    isSubmitButtonLoading.value = false;
    // @ts-expect-error: vue-next #4397
    submitButtonRef.value?.holdDisabled();
  });
}
</script>
