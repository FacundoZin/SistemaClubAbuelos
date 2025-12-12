<script setup>
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import SocioFormModal from '../components/SocioFormModal.vue'
import SocioCard from '../components/SocioCard.vue'
import SocioList from '../components/SocioList.vue'

// State
const currentAction = ref('none') // 'none', 'add', 'search', 'debtors'
const router = useRouter()

// Modal State
const isModalOpen = ref(false)
const selectedSocio = ref(null) // For editing

// Search State
const searchDni = ref('')
const searchResult = ref(null)
const searchError = ref('')
const isSearching = ref(false)

// Debtors State
const debtorsList = ref([])
const isLoadingDebtors = ref(false)
const debtorsError = ref('')

// Actions Configuration
const actions = [
  {
    id: 'add',
    title: 'Agregar socio',
    description: 'Registrar un nuevo socio en el padrón.',
    icon: 'M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z',
    color: 'text-blue-600',
    bg: 'bg-blue-50',
    hoverBorder: 'group-hover:border-blue-200'
  },
  {
    id: 'search',
    title: 'Buscar socio por DNI',
    description: 'Consultar estado e información de un socio.',
    icon: 'M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z',
    color: 'text-indigo-600',
    bg: 'bg-indigo-50',
    hoverBorder: 'group-hover:border-indigo-200'
  },
  {
    id: 'debtors',
    title: 'Ver socios deudores',
    description: 'Listado de socios con cuotas pendientes.',
    icon: 'M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z',
    color: 'text-rose-600',
    bg: 'bg-rose-50',
    hoverBorder: 'group-hover:border-rose-200'
  }
]

const selectAction = (actionId) => {
  currentAction.value = actionId
  // Reset states
  searchResult.value = null
  searchDni.value = ''
  searchError.value = ''
  debtorsList.value = []
  debtorsError.value = ''
  
  if (actionId === 'add') {
    openModal()
  } else if (actionId === 'debtors') {
    fetchDebtors()
  }
}

const goHome = () => {
  router.push('/')
}

// Modal Logic
const openModal = (socio = null) => {
  selectedSocio.value = socio
  isModalOpen.value = true
}

const closeModal = () => {
  isModalOpen.value = false
  selectedSocio.value = null
  // If we were in 'add' mode and cancelled without saving, maybe go back to none? 
  // But user might want to try again. Let's keep currentAction.
}

const handleSaveSocio = (savedSocio) => {
  closeModal()
  // Refresh data if needed
  if (currentAction.value === 'search' && searchResult.value && searchResult.value.id === savedSocio.id) {
    searchResult.value = savedSocio
  } else if (currentAction.value === 'debtors') {
    fetchDebtors()
  } else if (currentAction.value === 'add') {
      // Show success feedback or switch to search to see the new user?
      // For now just stay on add but maybe clear selection
      alert('Socio guardado correctamente')
  }
}

// Search Logic
const handleSearch = async () => {
  if (!searchDni.value) return
  
  isSearching.value = true
  searchError.value = ''
  searchResult.value = null

  try {
    const response = await fetch(`http://localhost:5194/api/Socios/${searchDni.value}`)
    
    if (response.status === 404) {
      searchError.value = 'No se encontró ningún socio con ese DNI'
      return
    }
    
    if (!response.ok) {
        // Try to parse error message
        const errorData = await response.json().catch(() => ({}))
        throw new Error(errorData.mensaje || 'Error al buscar socio')
    }

    const data = await response.json()
    searchResult.value = data
  } catch (error) {
    searchError.value = error.message
  } finally {
    isSearching.value = false
  }
}

// Debtors Logic
const fetchDebtors = async () => {
  isLoadingDebtors.value = true
  debtorsError.value = ''
  
  try {
    const response = await fetch('http://localhost:5194/api/Socios/deudores')
    if (!response.ok) {
      throw new Error('Error al obtener lista de deudores')
    }
    const data = await response.json()
    debtorsList.value = data
  } catch (error) {
    debtorsError.value = error.message
  } finally {
    isLoadingDebtors.value = false
  }
}

// Card Actions
const handleEdit = (socio) => {
  openModal(socio)
}

const handleDelete = async (socio) => {
  if (!confirm(`¿Está seguro que desea dar de baja a ${socio.nombre} ${socio.apellido}?`)) return

  try {
    const response = await fetch(`http://localhost:5194/api/Socios/${socio.id}`, {
      method: 'DELETE'
    })

    if (!response.ok) {
      throw new Error('Error al eliminar socio')
    }

    // Refresh
    if (currentAction.value === 'search') {
      searchResult.value = null
      searchDni.value = ''
      alert('Socio eliminado correctamente')
    } else if (currentAction.value === 'debtors') {
      fetchDebtors()
    }

  } catch (error) {
    alert(error.message)
  }
}

const handleView = (socio) => {
  alert(`Ver información completa de: ${socio.nombre} ${socio.apellido} (Pendiente de implementar vista detallada)`)
}
</script>

<template>
  <div class="min-h-screen bg-slate-50 font-sans text-slate-800">
    
    <!-- Header Institucional (Consistent with Home) -->
    <header class="bg-white border-b border-slate-200 sticky top-0 z-30 shadow-sm">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between h-16 items-center">
          <!-- Logo / Título -->
          <div class="flex items-center gap-3 cursor-pointer" @click="goHome">
            <div class="w-9 h-9 bg-blue-700 rounded-lg flex items-center justify-center shadow-md text-white">
               <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                 <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4" />
               </svg>
            </div>
            <div>
              <h1 class="text-lg font-bold text-slate-900 tracking-tight leading-none">Sistema Club Abuelos</h1>
              <span class="text-xs text-slate-500 font-medium">Panel de Administración</span>
            </div>
          </div>

          <!-- User Info -->
          <div class="flex items-center gap-6">
             <div class="hidden md:flex flex-col items-end">
                <span class="text-xs font-semibold text-slate-700">Administrador</span>
                <span class="text-[10px] text-slate-400 uppercase tracking-wider">
                  {{ new Date().toLocaleDateString('es-AR', { weekday: 'long', day: 'numeric', month: 'short' }) }}
                </span>
             </div>
             <div class="h-9 w-9 rounded-full bg-slate-100 border border-slate-200 flex items-center justify-center text-slate-600 text-xs font-bold shadow-sm ring-2 ring-white">
                AD
             </div>
          </div>
        </div>
      </div>
    </header>

    <!-- Main Content -->
    <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      
      <!-- Breadcrumb & Page Title -->
      <div class="mb-8">
        <nav class="flex mb-2" aria-label="Breadcrumb">
          <ol class="inline-flex items-center space-x-1 md:space-x-3">
            <li class="inline-flex items-center">
              <a href="#" @click.prevent="goHome" class="inline-flex items-center text-sm font-medium text-slate-500 hover:text-blue-600">
                <svg class="w-3 h-3 mr-2.5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 20 20">
                  <path d="m19.707 9.293-2-2-7-7a1 1 0 0 0-1.414 0l-7 7-2 2a1 1 0 0 0 1.414 1.414L2 10.414V18a2 2 0 0 0 2 2h3a1 1 0 0 0 1-1v-4a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1v4a1 1 0 0 0 1 1h3a2 2 0 0 0 2-2v-7.586l.293.293a1 1 0 0 0 1.414-1.414Z"/>
                </svg>
                Inicio
              </a>
            </li>
            <li>
              <div class="flex items-center">
                <svg class="w-3 h-3 text-slate-400 mx-1" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 6 10">
                  <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m1 9 4-4-4-4"/>
                </svg>
                <span class="ml-1 text-sm font-medium text-slate-700 md:ml-2">Gestión de Socios</span>
              </div>
            </li>
          </ol>
        </nav>
        <h2 class="text-3xl font-bold text-slate-900 tracking-tight">Gestión de Socios</h2>
        <p class="text-slate-500 mt-1 text-lg">Seleccione una acción para administrar socios.</p>
      </div>

      <!-- Action Cards -->
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 mb-10">
        <button 
          v-for="action in actions" 
          :key="action.id"
          @click="selectAction(action.id)"
          class="group relative flex flex-col p-6 bg-white rounded-xl border border-slate-200 shadow-sm hover:shadow-lg hover:-translate-y-1 transition-all duration-300 text-left overflow-hidden"
          :class="[action.hoverBorder, currentAction === action.id ? 'ring-2 ring-blue-500 border-transparent' : '']"
        >
          <!-- Hover Background Effect -->
          <div class="absolute inset-0 bg-gradient-to-br from-slate-50 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300"></div>

          <div class="relative z-10 flex items-start justify-between mb-5">
            <div class="p-3.5 rounded-lg transition-colors duration-300 shadow-sm ring-1 ring-black/5" :class="[action.bg, action.color]">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-7 w-7" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" :d="action.icon" />
              </svg>
            </div>
          </div>

          <div class="relative z-10 mt-auto">
            <h3 class="text-lg font-bold text-slate-900 mb-1 group-hover:text-blue-700 transition-colors duration-300">
              {{ action.title }}
            </h3>
            <p class="text-sm text-slate-500 leading-relaxed font-medium">
              {{ action.description }}
            </p>
          </div>
        </button>
      </div>

      <!-- Dynamic Content Area -->
      <div v-if="currentAction !== 'none'" class="bg-white rounded-xl border border-slate-200 shadow-sm p-6 min-h-[400px]">
        
        <!-- ADD ACTION (Handled by Modal mostly, but showing placeholder or instructions) -->
        <div v-if="currentAction === 'add'" class="flex flex-col items-center justify-center h-full py-12 text-center">
            <div class="p-4 bg-blue-50 rounded-full mb-4">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8 text-blue-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z" />
              </svg>
            </div>
            <h3 class="text-lg font-medium text-slate-900">Registrar nuevo socio</h3>
            <p class="text-slate-500 mt-2 max-w-md">El formulario se ha abierto en una ventana emergente. Si la cerró, puede volver a abrirla con el botón de abajo.</p>
            <button @click="openModal()" class="mt-6 inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500">
              Abrir formulario
            </button>
        </div>

        <!-- SEARCH ACTION -->
        <div v-else-if="currentAction === 'search'" class="max-w-2xl mx-auto">
            <div class="mb-8">
              <label for="search-dni" class="block text-sm font-medium text-slate-700 mb-2">Ingrese DNI del socio</label>
              <div class="flex gap-2">
                <div class="relative flex-grow">
                  <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                    <svg class="h-5 w-5 text-slate-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true">
                      <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" />
                    </svg>
                  </div>
                  <input 
                    type="text" 
                    id="search-dni" 
                    v-model="searchDni" 
                    @keyup.enter="handleSearch"
                    class="block w-full pl-10 pr-3 py-2 border border-slate-300 rounded-md leading-5 bg-white placeholder-slate-500 focus:outline-none focus:placeholder-slate-400 focus:ring-1 focus:ring-blue-500 focus:border-blue-500 sm:text-sm" 
                    placeholder="Ej: 12345678"
                  >
                </div>
                <button 
                  @click="handleSearch" 
                  :disabled="isSearching"
                  class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50"
                >
                  {{ isSearching ? 'Buscando...' : 'Buscar' }}
                </button>
              </div>
            </div>

            <!-- Search Results -->
            <div v-if="searchError" class="rounded-md bg-red-50 p-4 mb-6">
              <div class="flex">
                <div class="flex-shrink-0">
                  <svg class="h-5 w-5 text-red-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true">
                    <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clip-rule="evenodd" />
                  </svg>
                </div>
                <div class="ml-3">
                  <h3 class="text-sm font-medium text-red-800">No se encontró el socio</h3>
                  <div class="mt-2 text-sm text-red-700">
                    <p>{{ searchError }}</p>
                  </div>
                </div>
              </div>
            </div>

            <div v-if="searchResult">
              <SocioCard 
                :socio="searchResult" 
                @edit="handleEdit"
                @delete="handleDelete"
                @view="handleView"
              />
            </div>
        </div>

        <!-- DEBTORS ACTION -->
        <div v-else-if="currentAction === 'debtors'">
             <div class="flex justify-between items-center mb-6">
               <h3 class="text-lg font-medium text-slate-900">Socios con deuda pendiente</h3>
               <button @click="fetchDebtors" class="text-sm text-blue-600 hover:text-blue-800 font-medium">Actualizar lista</button>
             </div>

             <div v-if="isLoadingDebtors" class="flex justify-center py-12">
               <svg class="animate-spin h-8 w-8 text-blue-600" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                 <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                 <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
               </svg>
             </div>

             <div v-else-if="debtorsError" class="text-center py-12 text-red-600">
               {{ debtorsError }}
             </div>

             <SocioList 
               v-else 
               :socios="debtorsList"
               @edit="handleEdit"
               @delete="handleDelete"
               @view="handleView"
             />
        </div>
      </div>

    </main>

    <!-- Modal Component -->
    <SocioFormModal 
      :is-open="isModalOpen" 
      :socio="selectedSocio"
      @close="closeModal"
      @save="handleSaveSocio"
    />

  </div>
</template>
