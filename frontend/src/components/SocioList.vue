<script setup>
import SocioCard from './SocioCard.vue'

defineProps({
  socios: {
    type: Array,
    default: () => []
  }
})

defineEmits(['edit', 'delete', 'view'])
</script>

<template>
  <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
    <SocioCard 
      v-for="socio in socios" 
      :key="socio.id" 
      :socio="socio"
      @edit="$emit('edit', $event)"
      @delete="$emit('delete', $event)"
      @view="$emit('view', $event)"
    />
    
    <div v-if="socios.length === 0" class="col-span-full py-12 text-center text-slate-500">
      No se encontraron socios para mostrar.
    </div>
  </div>
</template>
