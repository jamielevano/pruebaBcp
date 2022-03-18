using ApiTipoCambio.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiTipoCambio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoCambioController : ControllerBase
    {
        private readonly TipoCambioDbContext _ctx;

        public TipoCambioController(TipoCambioDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpPost]
        [Route("CargarTC")]
        [Authorize]
        public IActionResult agregarTC()
        {
            TipoCambio tcUSD = new TipoCambio()
            {
                id = _ctx.TiposCambios.Count() + 1,
                monedaOrigen = "USD",
                monedaDestino = "PEN",
                tcCompra = 3.712M,
                tcVenta = 3.891M
            };
            _ctx.TiposCambios.Add(tcUSD);
            _ctx.SaveChanges();

            TipoCambio tcEUR = new TipoCambio()
            {
                id = _ctx.TiposCambios.Count() + 1,
                monedaOrigen = "EUR",
                monedaDestino = "PEN",
                tcCompra = 4.05M,
                tcVenta = 4.14M
            };
            _ctx.TiposCambios.Add(tcEUR);
            _ctx.SaveChanges();

            return Ok("TC Cargados.");
        }

        [HttpGet]
        public List<TipoCambio> GetTipoCambios()
        {
            return _ctx.TiposCambios.ToList();
        }

        [HttpGet("{id}")]
        public TipoCambio GetTipoCambiosById(int id)
        {
            return _ctx.TiposCambios.SingleOrDefault(x => x.id == id);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddTipoCambio(TipoCambio obj)
        {
            if (obj != null)
            {
                obj.id = _ctx.TiposCambios.Count() + 1;
                _ctx.TiposCambios.Add(obj);
                _ctx.SaveChanges();
                return Created("api/TipoCambio/" + obj.id, obj);
            }
            else
            {
                return NotFound("Formato del objeto no valido.");
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateTipoCambio(TipoCambio obj)
        {
            if (obj != null)
            {
                if(obj.tcCompra == 0 || obj.tcVenta == 0)
                    return NotFound("Tipo de Cambio no puede tener valor 0.");

                var tc = _ctx.TiposCambios.SingleOrDefault(x => x.id == obj.id);
                if (tc != null)
                {
                    tc.monedaDestino = obj.monedaDestino;
                    tc.monedaOrigen = obj.monedaOrigen;
                    tc.tcCompra = obj.tcCompra;
                    tc.tcVenta = obj.tcVenta;

                    _ctx.SaveChanges();
                    return Ok("Tipo cambio actualizado.");
                }
                else
                {
                    return NotFound("Tipo de Cambio no fue actualizado.");
                }
            }
            else
            {
                return NotFound("Formato del objeto no valido.");
            }

        }

        [HttpPost]
        [Authorize]
        [Route("CambiarMonedas")]
        public IActionResult OpeTipoCambio(OperacionTC obj)
        {
            if (obj != null)
            {
                var tipoCambio = _ctx.TiposCambios.SingleOrDefault(x => x.monedaOrigen.Trim().ToUpper() == obj.monedaOrigen.Trim().ToUpper() &&
                                                                        x.monedaDestino.Trim().ToUpper() == obj.monedaDestino.Trim().ToUpper());

                var tipoCambioAux = _ctx.TiposCambios.SingleOrDefault(x => x.monedaOrigen.Trim().ToUpper() == obj.monedaDestino.Trim().ToUpper() &&
                                                                        x.monedaDestino.Trim().ToUpper() == obj.monedaOrigen.Trim().ToUpper());

                decimal tc = 0;
                string accion = "";

                if (!String.IsNullOrEmpty(obj.accion))
                {
                    accion = obj.accion.Trim().ToUpper();
                }
                else {
                    accion = obj.accion = "C";
                }
                    

                if (tipoCambio != null)
                {
                    if (accion == "C")
                        tc = tipoCambio.tcCompra;
                    else if (accion == "V")
                        tc = tipoCambio.tcVenta;
                }
                else if (tipoCambioAux != null)
                {
                    if (accion == "C")
                        tc = decimal.Round(1 / tipoCambioAux.tcCompra, 2);
                    else if (accion == "V")
                        tc = decimal.Round(1 / tipoCambioAux.tcVenta, 2);
                }

                if (tc > 0)
                {
                    obj.montoTc = obj.monto * tc;
                    obj.tipoCambio = tc;
                    obj.fechaOperacion = DateTime.Now;
                    obj.id = _ctx.OperacionesTC.Count() + 1;

                    _ctx.OperacionesTC.Add(obj);
                    _ctx.SaveChanges();
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    return Ok(json);
                }
                else
                    return NotFound("No se encotró TC. Operación terminada.");
            }
            else {
                return NotFound("Formato del objeto no valido.");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetCambiosMonedas")]
        public List<OperacionTC> GetOpeTipoCambios()
        {
            return _ctx.OperacionesTC.ToList();
        }
    }
}
