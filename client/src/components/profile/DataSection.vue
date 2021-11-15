<template>
  <n-h4>
    <n-text type="primary">
      Изменение email
    </n-text>
  </n-h4>
  <email-editing-form />
  <n-h4>
    <n-text type="primary">
      Изменение пароля
    </n-text>
  </n-h4>
  <password-editing-form />
</template>

<script setup lang="ts">
import EmailEditingForm from '@/components/profile/EmailEditingForm.vue'
import PasswordEditingForm from '@/components/profile/PasswordEditingForm.vue'
</script>
