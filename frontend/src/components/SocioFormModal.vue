<script setup>
import { ref, reactive } from 'vue'

const props = defineProps({
  isOpen: Boolean,
  socio: {
    type: Object,
    default: null
  }
})

const emit = defineEmits(['close', 'save'])

const form = reactive({
  nombre: '',
  apellido: '',
  dni: '',
  telefono: '',
  direcccion: '', // Note: keeping typo to match DTO
  lote: '',
  localidad: ''
})

const isSubmitting = ref(false)
const errorMessage = ref('')

const resetForm = () => {
  form.nombre = ''
  form.apellido = ''
  form.dni = ''
  form.telefono = ''
  form.direcccion = ''
  form.lote = ''
  form.localidad = ''
  errorMessage.value = ''
}

const handleSubmit = async () => {
  isSubmitting.value = true
  errorMessage.value = ''

  try {
    const response = await fetch('http://localhost:5194/api/Socios', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(form)
    })

    if (!response.ok) {
      throw new Error('Error al guardar el socio')
    }

    const data = await response.json()
    emit('save', data)
    resetForm()
  } catch (error) {
    errorMessage.value = error.message
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <div v-if="isOpen" class="fixed inset-0 z-50 overflow-y-auto" aria-labelledby="modal-title" role="dialog" aria-modal="true">
    <!-- Background backdrop with blur -->
    <div class="fixed inset-0 bg-slate-900/30 backdrop-blur-sm transition-opacity" @click="$emit('close')"></div>

    <div class="flex min-h-full items-center justify-center p-4 text-center sm:p-0">
      <div class="relative transform overflow-hidden rounded-lg bg-white text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg border border-slate-200">
        
        <!-- Header -->
        <div class="bg-white px-4 pb-4 pt-5 sm:p-6 sm:pb-4 border-b border-slate-100">
          <div class="sm:flex sm:items-start">
            <div class="mx-auto flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full bg-blue-100 sm:mx-0 sm:h-10 sm:w-10">
              <svg class="h-6 w-6 text-blue-600" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" aria-hidden="true">
                <path stroke-linecap="round" stroke-linejoin="round" d="M19 7.5v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z" />
              </svg>
            </div>
            <div class="mt-3 text-center sm:ml-4 sm:mt-0 sm:text-left">
              <h3 class="text-lg font-semibold leading-6 text-slate-900" id="modal-title">Registrar Nuevo Socio</h3>
              <div class="mt-2">
                <p class="text-sm text-slate-500">Complete la información para dar de alta un nuevo socio en el sistema.</p>
              </div>
            </div>
          </div>
        </div>

        <!-- Form -->
        <form @submit.prevent="handleSubmit">
          <div class="px-4 py-5 sm:p-6 space-y-4">
            
            <div v-if="errorMessage" class="p-3 rounded-md bg-red-50 text-red-700 text-sm mb-4">
              {{ errorMessage }}
            </div>

            <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div>
                <label for="nombre" class="block text-sm font-medium text-slate-700">Nombre</label>
                <input type="text" id="nombre" v-model="form.nombre" required class="mt-1 block w-full rounded-md border-slate-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm px-3 py-2 border">
              </div>
              <div>
                <label for="apellido" class="block text-sm font-medium text-slate-700">Apellido</label>
                <input type="text" id="apellido" v-model="form.apellido" required class="mt-1 block w-full rounded-md border-slate-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm px-3 py-2 border">
              </div>
            </div>

            <div>
              <label for="dni" class="block text-sm font-medium text-slate-700">DNI</label>
              <input type="text" id="dni" v-model="form.dni" required class="mt-1 block w-full rounded-md border-slate-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm px-3 py-2 border">
            </div>

            <div>
              <label for="telefono" class="block text-sm font-medium text-slate-700">Teléfono</label>
              <input type="tel" id="telefono" v-model="form.telefono" class="mt-1 block w-full rounded-md border-slate-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm px-3 py-2 border">
            </div>

            <div>
              <label for="direccion" class="block text-sm font-medium text-slate-700">Dirección</label>
              <input type="text" id="direccion" v-model="form.direcccion" class="mt-1 block w-full rounded-md border-slate-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm px-3 py-2 border">
            </div>

            <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div>
                <label for="lote" class="block text-sm font-medium text-slate-700">Lote</label>
                <input type="text" id="lote" v-model="form.lote" class="mt-1 block w-full rounded-md border-slate-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm px-3 py-2 border">
              </div>
              <div>
                <label for="localidad" class="block text-sm font-medium text-slate-700">Localidad</label>
                <input type="text" id="localidad" v-model="form.localidad" class="mt-1 block w-full rounded-md border-slate-300 shadow-sm focus:border-blue-500 focus:ring-blue-500 sm:text-sm px-3 py-2 border">
              </div>
            </div>

          </div>

          <!-- Footer Actions -->
          <div class="bg-slate-50 px-4 py-3 sm:flex sm:flex-row-reverse sm:px-6 border-t border-slate-200">
            <button type="submit" :disabled="isSubmitting" class="inline-flex w-full justify-center rounded-md bg-blue-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-blue-500 sm:ml-3 sm:w-auto disabled:opacity-50 disabled:cursor-not-allowed transition-colors">
              {{ isSubmitting ? 'Guardando...' : 'Guardar Socio' }}
            </button>
            <button type="button" @click="$emit('close')" class="mt-3 inline-flex w-full justify-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-slate-900 shadow-sm ring-1 ring-inset ring-slate-300 hover:bg-slate-50 sm:mt-0 sm:w-auto transition-colors">
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>
