using PersonalFinance.Identity.Domain.Entities;

namespace PersonalFinance.Identity.UnitTests.Domain;

public class UserTests
{
    [Fact]
    public void Novo_usuario_comeca_ativo_e_com_email_pendente()
    {
        var usuario = CriarUsuario();

        Assert.True(usuario.IsActive);
        Assert.False(usuario.EmailConfirmed);
        Assert.Equal(1, usuario.TokenVersion);
        Assert.NotEqual(Guid.Empty, usuario.Id);
    }

    [Fact]
    public void Troca_de_senha_invalida_tokens_anteriores()
    {
        var usuario = CriarUsuario();
        var versaoAnterior = usuario.TokenVersion;

        usuario.ChangePassword("novo-hash-de-exemplo");

        Assert.Equal("novo-hash-de-exemplo", usuario.PasswordHash);
        Assert.Equal(versaoAnterior + 1, usuario.TokenVersion);
        Assert.NotNull(usuario.UpdatedAt);
    }

    [Fact]
    public void Desativacao_e_idempotente_e_invalida_tokens_apenas_na_primeira_vez()
    {
        var usuario = CriarUsuario();

        usuario.Deactivate();
        var versaoAposDesativar = usuario.TokenVersion;
        usuario.Deactivate();

        Assert.False(usuario.IsActive);
        Assert.Equal(2, versaoAposDesativar);
        Assert.Equal(versaoAposDesativar, usuario.TokenVersion);
    }

    [Fact]
    public void Confirmacao_de_email_e_idempotente()
    {
        var usuario = CriarUsuario();

        usuario.ConfirmEmail();
        var ultimaAlteracao = usuario.UpdatedAt;
        usuario.ConfirmEmail();

        Assert.True(usuario.EmailConfirmed);
        Assert.Equal(ultimaAlteracao, usuario.UpdatedAt);
    }

    private static User CriarUsuario()
    {
        var endereco = new Endereco("Rua de Teste", "10", null, "Centro", "Petrópolis", "RJ", "25600000", "Brasil");
        return new User("Teste", "Usuario", new DateTime(1992, 1, 1), "usuario@example.org", "hash-ficticio", endereco);
    }
}
