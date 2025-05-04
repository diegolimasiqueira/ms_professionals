using MSProfessionals.Domain.Entities;

namespace MSProfessionals.Domain.Interfaces;

/// <summary>
/// Interface para o repositório de endereços
/// </summary>
public interface IAddressRepository
{
    /// <summary>
    /// Adiciona um novo endereço
    /// </summary>
    /// <param name="address">O endereço a ser adicionado</param>
    /// <returns>Task</returns>
    Task AddAsync(ProfessionalAddress professionalAddress);

    /// <summary>
    /// Atualiza um endereço existente
    /// </summary>
    /// <param name="address">O endereço a ser atualizado</param>
    /// <returns>Task</returns>
    Task UpdateAsync(ProfessionalAddress professionalAddress);

    /// <summary>
    /// Remove um endereço
    /// </summary>
    /// <param name="address">O endereço a ser removido</param>
    /// <returns>Task</returns>
    Task DeleteAsync(ProfessionalAddress professionalAddress);

    /// <summary>
    /// Obtém um endereço pelo ID
    /// </summary>
    /// <param name="id">ID do endereço</param>
    /// <returns>O endereço encontrado ou null</returns>
    Task<ProfessionalAddress?> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtém todos os endereços de um consumidor
    /// </summary>
    /// <param name="professionalId">ID do profissional</param>
    /// <returns>Lista de endereços do profissional</returns>
    Task<IEnumerable<ProfessionalAddress>> GetByProfessionalIdAsync(Guid professionalId);

    /// <summary>
    /// Obtém o endereço principal de um profissional
    /// </summary>
    /// <param name="professionalId">ID do profissional</param>
    /// <returns>O endereço principal ou null</returns>
    Task<ProfessionalAddress?> GetDefaultByProfessionalIdAsync(Guid professionalId);
} 