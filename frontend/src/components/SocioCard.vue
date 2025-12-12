<script setup>
defineProps({
  socio: {
    type: Object,
    required: true
  }
})

defineEmits(['edit', 'delete', 'view'])
</script>

<template>
  <div class="bg-white rounded-lg border border-slate-200 shadow-sm hover:shadow-md transition-shadow duration-200 overflow-hidden">
    <div class="p-5">
      <div class="flex justify-between items-start">
        <div>
          <h3 class="text-lg font-bold text-slate-900">{{ socio.nombre }} {{ socio.apellido }}</h3>
          <p class="text-sm text-slate-500 font-medium mt-1">DNI: {{ socio.dni }}</p>
        </div>
        <span 
          class="inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium"
          :class="socio.adeudaCuotas ? 'bg-red-100 text-red-800' : 'bg-green-100 text-green-800'"
        >
          {{ socio.adeudaCuotas ? 'Deudor' : 'Al día' }}
        </span>
      </div>
      
      <div class="mt-4 space-y-2">
        <div class="flex items-center text-sm text-slate-600">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 mr-2 text-slate-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z" />
          </svg>
          {{ socio.telefono || 'Sin teléfono' }}
        </div>
        <div class="flex items-center text-sm text-slate-600">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 mr-2 text-slate-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
          </svg>
          {{ socio.direcccion || socio.localidad || 'Sin dirección' }}
        </div>
      </div>
    </div>

    <div class="bg-slate-50 px-5 py-3 border-t border-slate-100 flex justify-end gap-2">
      <button 
        @click="$emit('view', socio)"
        class="text-xs font-medium text-slate-600 hover:text-blue-600 px-2 py-1 rounded hover:bg-slate-200 transition-colors"
      >
        Ver info
      </button>
      <button 
        @click="$emit('edit', socio)"
        class="text-xs font-medium text-blue-600 hover:text-blue-800 px-2 py-1 rounded hover:bg-blue-50 transition-colors"
      >
        Editar
      </button>
      <button 
        @click="$emit('delete', socio)"
        class="text-xs font-medium text-red-600 hover:text-red-800 px-2 py-1 rounded hover:bg-red-50 transition-colors"
      >
        Baja
      </button>
    </div>
  </div>
</template>
