package api.web.movile.model;

import jakarta.persistence.*;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "ventas")
public class Venta {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "fecha_venta", insertable = false, updatable = false)
    private LocalDateTime fechaVenta;

    private Double total;

    @Column(name = "cliente_nombre")
    private String clienteNombre;

    @OneToMany(mappedBy = "venta", cascade = CascadeType.ALL)
    private List<DetalleVenta> detalles = new ArrayList<>();

    // Getters y Setters
    public Long getId() { return id; }
    public void setId(Long id) { this.id = id; }
    public LocalDateTime getFechaVenta() { return fechaVenta; }
    public Double getTotal() { return total; }
    public void setTotal(Double total) { this.total = total; }
    public String getClienteNombre() { return clienteNombre; }
    public void setClienteNombre(String clienteNombre) { this.clienteNombre = clienteNombre; }
    public List<DetalleVenta> getDetalles() { return detalles; }
    public void setDetalles(List<DetalleVenta> detalles) {
        this.detalles = detalles;
        for(DetalleVenta d : detalles) d.setVenta(this);
    }
}