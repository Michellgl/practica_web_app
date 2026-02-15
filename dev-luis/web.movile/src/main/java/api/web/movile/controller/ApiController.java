package api.web.movile.controller;

import api.web.movile.model.Articulo;
import api.web.movile.model.Venta;
import api.web.movile.repository.ArticuloRepository;
import api.web.movile.repository.VentaRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api")
public class ApiController {

    @Autowired
    private ArticuloRepository articuloRepository;

    @Autowired
    private VentaRepository ventaRepository;

    // 1. Consultar Artículos (GET)
    @GetMapping("/articulos")
    public List<Articulo> listarArticulos() {
        return articuloRepository.findAll();
    }

    // 2. Realizar Compra (POST)
    @PostMapping("/comprar")
    public ResponseEntity<String> procesarCompra(@RequestBody Venta venta) {
        try {
            ventaRepository.save(venta);
            return ResponseEntity.ok("Compra exitosa. ID Venta: " + venta.getId());
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Error al procesar compra: " + e.getMessage());
        }
    }
}