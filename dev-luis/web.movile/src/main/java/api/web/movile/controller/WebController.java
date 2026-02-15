package api.web.movile.controller;

import api.web.movile.model.Articulo;
import api.web.movile.repository.ArticuloRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping("/admin")
public class WebController {

    @Autowired
    private ArticuloRepository articuloRepository;

    // 1. Ver lista
    @GetMapping("/inventario")
    public String verInventario(Model model) {
        model.addAttribute("listaArticulos", articuloRepository.findAll());
        return "inventario";
    }

    // 2. Formulario para CREAR NUEVO
    @GetMapping("/nuevo")
    public String mostrarFormulario(Model model) {
        model.addAttribute("articulo", new Articulo());
        return "formulario";
    }

    // 3. Formulario para EDITAR (NUEVO)
    @GetMapping("/editar/{id}")
    public String editarArticulo(@PathVariable Long id, Model model) {
        // Buscamos el artículo por ID. Si existe, lo mandamos al formulario.
        Articulo articulo = articuloRepository.findById(id).orElse(null);

        if (articulo != null) {
            model.addAttribute("articulo", articulo);
            return "formulario"; // Reusamos el mismo html de formulario
        }

        return "redirect:/admin/inventario"; // Si no existe, volvemos a la lista
    }

    // 4. Guardar (Sirve para Crear y para Editar)
    @PostMapping("/guardar")
    public String guardarArticulo(@ModelAttribute Articulo articulo) {
        // Validación básica de imagen
        if (articulo.getImagenUrl() == null || articulo.getImagenUrl().isEmpty()) {
            articulo.setImagenUrl("https://ui-avatars.com/api/?name=" + articulo.getNombre());
        }
        articuloRepository.save(articulo); // .save() detecta solo si es Update o Insert por el ID
        return "redirect:/admin/inventario";
    }

    // 5. Eliminar
    @GetMapping("/eliminar/{id}")
    public String eliminarArticulo(@PathVariable Long id) {
        articuloRepository.deleteById(id);
        return "redirect:/admin/inventario";
    }
}