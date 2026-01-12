using DOSFinal.Application.DTOs;
using DOSFinal.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DOSFinal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReservationsController> _logger;

    public ReservationsController(IReservationService reservationService, ILogger<ReservationsController> logger)
    {
        _reservationService = reservationService;
        _logger = logger;
    }

    /// <summary>
    /// Get all reservations or filter by date
    /// </summary>
    /// <param name="date">Optional date filter (format: YYYY-MM-DD)</param>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReservationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetAll([FromQuery] DateOnly? date)
    {
        try
        {
            IEnumerable<ReservationDto> reservations;
            
            if (date.HasValue)
            {
                reservations = await _reservationService.GetByDateAsync(date.Value);
            }
            else
            {
                reservations = await _reservationService.GetAllAsync();
            }

            return Ok(reservations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reservations");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Get a specific reservation by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservationDto>> GetById(int id)
    {
        try
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null)
                return NotFound(new { error = $"Reservation with ID {id} not found" });

            return Ok(reservation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reservation {ReservationId}", id);
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Create a new reservation
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ReservationDto>> Create([FromBody] CreateReservationDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.CustomerName))
                return BadRequest(new { error = "Customer name is required" });

            if (dto.TableNumber <= 0)
                return BadRequest(new { error = "Table number must be positive" });

            if (dto.NumberOfPeople <= 0)
                return BadRequest(new { error = "Number of people must be positive" });

            var reservation = await _reservationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Conflict creating reservation");
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating reservation");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Update an existing reservation
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ReservationDto>> Update(int id, [FromBody] UpdateReservationDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.CustomerName))
                return BadRequest(new { error = "Customer name is required" });

            if (dto.TableNumber <= 0)
                return BadRequest(new { error = "Table number must be positive" });

            if (dto.NumberOfPeople <= 0)
                return BadRequest(new { error = "Number of people must be positive" });

            var reservation = await _reservationService.UpdateAsync(id, dto);
            if (reservation == null)
                return NotFound(new { error = $"Reservation with ID {id} not found" });

            return Ok(reservation);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Conflict updating reservation {ReservationId}", id);
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating reservation {ReservationId}", id);
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Delete/Cancel a reservation
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _reservationService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { error = $"Reservation with ID {id} not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting reservation {ReservationId}", id);
            return StatusCode(500, new { error = "Internal server error" });
        }
    }
}
